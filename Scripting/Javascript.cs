using Jint;
using NgUtil.Debugging.Contracts;

namespace NgHTTP.Scripting {
    public static class Javascript {


        public static object ExecuteFunction(string jsCode, string functionName) {
            EmptyParamContract.Validate(jsCode);
            EmptyParamContract.Validate(functionName);

            return new Engine().Execute(jsCode)
                .GetValue(functionName)
                .ToObject();
        }

        public static object ExecuteScript(string jsCode) {
            EmptyParamContract.Validate(jsCode);

            return new Engine().Execute(jsCode)
                .GetCompletionValue()
                .ToObject();
        }

    }

}
