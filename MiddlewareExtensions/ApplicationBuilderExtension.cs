﻿using notification_system.Hubs;
using notification_system.SubscribeTableDependencies;

namespace notification_system.MiddlewareExtensions {
    public static class ApplicationBuilderExtension {
        public static void UseSqlTableDependency<T>(this IApplicationBuilder applicationBuilder, string connectionString, RequestHub requestHub)
                 where T : ISubscribeTableDependency {
            var serviceProvider = applicationBuilder.ApplicationServices;
            var service = serviceProvider.GetService<T>();
            service.SubscribeTableDependency(connectionString, requestHub);
        }
    }
}
