using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
using COSXML.Utils;
using Nito.AsyncEx;
using NLog;
using tesla_wpf.Extensions;
using tesla_wpf.Model.System.Storage;
using tesla_wpf.Rest;
using Vera.Wpf.Lib.Exceptions;
using Vera.Wpf.Lib.Helper;

namespace tesla_wpf.Model.System.Storage {
    /// <summary>
    /// 腾讯云上传服务
    /// </summary>
    public class TecentStorage : ICloudStorage {
        /// <summary>
        /// 下载图片的临时账号
        /// </summary>
        GetImageCredential getImageCredential = new GetImageCredential();
        /// <summary>
        /// 上传图片的临时账号
        /// </summary>
        PutImageCredential putImageCredential = new PutImageCredential();
        /// <summary>
        /// 删除图片的临时账号
        /// </summary>
        DelImageCredential delImageCredential = new DelImageCredential();
        /// <summary>
        /// 日志工具
        /// </summary>
        readonly Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 签名有效期
        /// </summary>
        readonly int signValidSec = 250;

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="fileKey">上传到云上面的 key</param>
        /// <param name="fileSrc">本地文件路径</param>
        /// <returns></returns>
        public async Task PutImage(string fileSrc, string fileKey) {
            var cosService = await buildCosService(putImageCredential, fileKey);
            var request = new PutObjectRequest(putImageCredential.TmpCred.Bucket, fileKey, fileSrc);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), signValidSec);
            //设置进度回调
            request.SetCosProgressCallback((c, t) => {
                double percent = (double)c / t * 100;
                logger.Trace("上传进度 {}%", percent.ToString("0.00"));
            });
            //执行请求
            PutObjectResult result = cosService.PutObject(request);
            if (result.httpCode != 200) {
                throw new StorageException(result.httpMessage);
            }
            //logger.Trace(result.GetResultInfo());
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="fileKey">图片 key</param>
        /// <param name="localDir">本地文件夹</param>
        /// <param name="localFileName">本地文件名</param>
        /// <returns></returns>
        public async Task GetImage(string fileKey, string localDir, string localFileName) {
            var tmpFile = "tmp_" + localFileName;
            var tmpFilePath = localDir + "/" + tmpFile;
            var localFilePath = localDir + "/" + localFileName;
            var cosService = await buildCosService(getImageCredential, fileKey);
            var request = new GetObjectRequest(getImageCredential.TmpCred.Bucket, fileKey, localDir, tmpFile);
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), signValidSec);
            request.SetCosProgressCallback((c, t) => {
                double percent = (double)c / t * 100;
                logger.Trace("下载进度 {}%", percent.ToString("0.00"));
            });
            try {
                var result = cosService.GetObject(request);
                //logger.Trace(result.GetResultInfo());
                // 下载成功，将临时文件移动到指定位置
                if (result.httpCode == 200) {
                    File.Move(tmpFilePath, localFilePath);
                    // 下载失败也会出现残留文件，这里将临时下载残留文件删除
                } else {
                    if (File.Exists(tmpFilePath)) {
                        File.Delete(tmpFilePath);
                    }
                    throw new StorageException(result.httpMessage);
                }
            } catch (Exception e) {
                // 下载失败也会出现残留文件，这里将它删除
                if (File.Exists(tmpFilePath)) {
                    File.Delete(tmpFilePath);
                }
                throw e;
            }
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="fileKey">图片 key</param>
        /// <returns></returns>
        public async Task DelImage(string fileKey) {
            var cosService = await buildCosService(delImageCredential, fileKey);
            var request = new DeleteObjectRequest(getImageCredential.TmpCred.Bucket, fileKey);
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), signValidSec);
            var result = cosService.DeleteObject(request);
            if (result.httpCode != 200) {
                throw new StorageException(result.httpMessage);
            }
            //logger.Trace(result.GetResultInfo());
        }


        /// <summary>
        /// 构建腾讯云操作服务
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="srcPath"></param>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        private async Task<CosXmlServer> buildCosService(AbstractQCloudCredential provider, string fileKey) {
            if (provider.IsTimeout()) {
                await provider.Init(fileKey);
            }
            var config = buildCosConfig(provider.TmpCred.Region, provider.TmpCred.AppId);
            return new CosXmlServer(config, provider);
        }
        /// <summary>
        /// 构建腾讯云云配置
        /// </summary>
        /// <param name="region"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        private CosXmlConfig buildCosConfig(string region, string appid) {
            return new CosXmlConfig.Builder()
               .SetConnectionTimeoutMs(60000)  //设置连接超时时间，单位 毫秒 ，默认 45000ms
               .SetReadWriteTimeoutMs(40000)  //设置读写超时时间，单位 毫秒 ，默认 45000ms
               .IsHttps(true)  //设置默认 https 请求
               .SetDebugLog(true)  //显示日志;
               .SetRegion(region)
               .SetAppid(appid)
               .Build();
        }


        /// <summary>
        /// 临时账号管理
        /// </summary>
        public abstract class AbstractQCloudCredential : QCloudCredentialProvider {
            QCloudCredentials cloudCredentials;
            public StorageTmpCredential TmpCred { get; set; }
            /// <summary>
            /// 系统调用
            /// </summary>
            /// <returns></returns>
            public override QCloudCredentials GetQCloudCredentials() {
                return cloudCredentials;
            }

            /// <summary>
            /// 
            /// </summary>
            public override void Refresh() {

            }

            /// <summary>
            /// 获取临时账号
            /// </summary>
            /// <returns></returns>
            protected abstract Task<Rest<StorageTmpCredential>> fetchTmpCred(string fileKey);
            /// <summary>
            /// 标志位，解决多线程带来的 bug
            /// </summary>
            public AsyncLock AsyncLocker = new AsyncLock();

            /// <summary>
            /// 初始化临时账号
            /// </summary>
            /// <param name="fileKey">操作文件 key，可以为空，删除资源的时候不能为空</param>
            public async Task Init(string fileKey) {
                using (await AsyncLocker.LockAsync()) {
                    // 防止重复初始化
                    if (!IsTimeout()) {
                        return;
                    }
                    var rest = await fetchTmpCred(fileKey);
                    if (!HttpRestService.ForData(rest, out var cred)) {
                        throw new RestFailedException(rest.Message);
                    }
                    TmpCred = cred;
                    var keyTime = $"{TimeHelper.GetTimestampSec(cred.StartTime)};{TimeHelper.GetTimestampSec(cred.ExpiredTime)}";
                    string signKey = DigestUtils.GetHamcSha1ToHexString(keyTime, Encoding.UTF8, cred.TmpSecretKey, Encoding.UTF8);
                    cloudCredentials = new SessionQCloudCredentials(
                        cred.TmpSecretId,
                        signKey,
                        cred.SessionToken,
                        keyTime
                        );
                }
            }

            /// <summary>
            /// 检查临时账号是否过期
            /// </summary>
            /// <returns></returns>
            public bool IsTimeout() {
                return TmpCred == null || DateTime.Now >= TmpCred.ExpiredTime;
            }
        }

        /// <summary>
        /// 临时下载图片权限
        /// </summary>
        public class GetImageCredential : AbstractQCloudCredential {
            protected override Task<Rest<StorageTmpCredential>> fetchTmpCred(string fileKey) {
                return HttpRestService.ForAuthApi<RsSystemApi>().FetchStorageGetImageCred();
            }
        }

        /// <summary>
        /// 临时上传图片权限
        /// </summary>
        public class PutImageCredential : AbstractQCloudCredential {
            protected override Task<Rest<StorageTmpCredential>> fetchTmpCred(string fileKey) {
                return HttpRestService.ForAuthApi<RsSystemApi>().FetchStoragePutImageCred();
            }
        }

        /// <summary>
        /// 临时删除图片权限
        /// </summary>
        public class DelImageCredential : AbstractQCloudCredential {
            protected override Task<Rest<StorageTmpCredential>> fetchTmpCred(string fileKey) {
                return HttpRestService.ForAuthApi<RsSystemApi>().FetchStorageDelImageCred(fileKey);
            }
        }
    }
}
