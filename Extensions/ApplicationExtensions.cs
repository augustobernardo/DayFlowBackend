namespace DayFlowAPI.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication UseApplicationExtensions(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "DayFlowAPI";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "DayFlow API - v1");
            });
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowAngular");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
