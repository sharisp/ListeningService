namespace Listening.Infrastructure
{
    public class LockHelper
    {
        public static async Task RedisLock(string lockKey, Func<Task>? lockFunc, Func<Task>? noLockFunc = null)
        {
            //var lockKey = "lock:user:permissions:";
            var token = Guid.NewGuid().ToString(); // 唯一标识当前锁的持有者
            int expireMs = 3000;

            bool locked = false;
            int retryCount = 0;

            while (!locked && retryCount < 3)
            {
                locked = RedisHelper.Set(lockKey, token, expireMs, CSRedis.RedisExistence.Nx);

                if (!locked)
                {
                    retryCount++;
                    await Task.Delay(200);
                }
            }
            if (locked)
            {
                try
                {
                    if (lockFunc != null)
                    {
                        await lockFunc();

                    }
                    // 查询数据库 & 写入缓存
                }
                finally
                {
                    // 解锁前确保是自己加的锁
                    var currentToken = RedisHelper.Get(lockKey);
                    if (currentToken == token)
                    {
                        RedisHelper.Del(lockKey);
                    }
                }
            }
            else
            {
                // 获取不到锁，可选择等待或快速失败
                if (noLockFunc != null)
                {
                    await noLockFunc();

                }
            }

        }
    }
}
