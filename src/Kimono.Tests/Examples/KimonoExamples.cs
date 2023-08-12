
namespace Kimono.Tests.Examples
{
    public class KimonoExamples
    {
        public void TypeInterceptorExample()
        {
            var withAction = Interceptor.WithAction<IFoo>(context =>
            {
                context.InvokeTarget();
            });

            

        }
    }

    public interface IFoo
    {
        void Bar();

        void Baz(string  a, string b);
    }
}
