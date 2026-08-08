using MediaToPacs.Core.Auths;
using MediaToPacs.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using STM.MediaToPACS.Main.Utilities;
using System;

namespace STM.MediaToPACS.Main.App
{
    /// <summary>
    /// Composition root: nơi duy nhất đăng ký service cho DI container.
    /// Thay thế dần vai trò khởi tạo service của ServiceLocator (đang giữ lại để chuyển đổi từng bước).
    /// </summary>
    public static class CompositionRoot
    {
        public static IServiceProvider Provider { get; private set; }

        /// <summary>
        /// Phải gọi SAU ServiceLocator.Initialize(). Đăng ký lại đúng những instance mà
        /// ServiceLocator đã tạo/đang cấu hình (gateway URL, token...), để tránh 2 bộ service
        /// lệch trạng thái nhau trong giai đoạn chuyển đổi sang DI.
        /// </summary>
        public static void Build()
        {
            var services = new ServiceCollection();

            services.AddSingleton(ServiceLocator.StudyService);
            services.AddSingleton(ServiceLocator.RisService);
            services.AddSingleton(ServiceLocator.RisService2);
            services.AddSingleton(ServiceLocator.SignatureService);
            services.AddSingleton(ServiceLocator.HisService);
            services.AddSingleton(ServiceLocator.SessionService);

            Provider = services.BuildServiceProvider();
        }

        public static T Resolve<T>()
        {
            if (Provider == null)
                throw new InvalidOperationException("CompositionRoot chưa được khởi tạo. Gọi CompositionRoot.Build() trước.");

            return Provider.GetRequiredService<T>();
        }
    }
}
