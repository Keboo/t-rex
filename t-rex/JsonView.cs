using Newtonsoft.Json;
using System;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using TRexLib;

namespace TRex.CommandLine
{
    public sealed class JsonView : ContentView
    {
        public JsonView(TestResultSet testResults)
        {
            if (testResults == null) throw new ArgumentNullException(nameof(testResults));
            
            var json = JsonConvert.SerializeObject(
                testResults,
                Formatting.Indented,
                new FileInfoJsonConverter(),
                new DirectoryInfoJsonConverter());

            Span = new ContentSpan(json);
        }
    }
}
