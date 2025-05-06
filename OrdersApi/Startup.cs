using Application.Interface;
using Application.Service;
using Confluent.Kafka;
using ExternalServices.Consumer;
using ExternalServices.KafkaConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Coconseconsentext;
using Repository.Interface;
using Repository.Repositories;

namespace OrdersApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<OrdersDb>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("PostgreConnectionString"));
            });

            services.Configure<KafkaSettings>
            (Configuration.GetSection(nameof(KafkaSettings)));

            services.AddTransient<IProcessOrderService, ProcessOrderService>();
            services.AddTransient<IProcessOrderRepository, ProccessOrderRepository>();
            services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            services.AddTransient<ICalculateOrderService, CalculateOrderService>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IEventConsumer, EventConsumer>();
            services.AddTransient<IProducer, Producer>();
            services.Configure<ConsumerConfig>(Configuration.GetSection(nameof(ConsumerConfig)));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
        }
    }
}
