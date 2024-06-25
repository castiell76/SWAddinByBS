using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using Azure;
using OpenAI.Chat;
using OpenAI.Assistants;
using System.Runtime.InteropServices;

namespace SWApp.Models
{

    [ComVisible(true)]
    class GPT
    {
        public GPT()
        {
            
        }

        public string GetGPT(string phraseToTranslate)
        {
            string apiKey = "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";
            ChatClient client = new(model: "gpt-3.5-turbo", apiKey);
            ChatCompletion completion = client.CompleteChat($"Przetłumacz wyrażenie: {phraseToTranslate}");
            //string keyFromEnvironment = Environment.GetEnvironmentVariable(apiKey);
            //AzureOpenAIClient azureClient = new(
            //new Uri("https://your-azure-openai-resource.com"),
            //new AzureKeyCredential(keyFromEnvironment));


            //ChatClient chatClient = azureClient.GetChatClient("my-gpt-35-turbo-deployment");
            //ChatCompletion completion = chatClient.CompleteChat(
            //[
            //    new SystemChatMessage("Przetłumacz podane zdanie na język angielski"),
            //    new UserChatMessage(phraseToTranslate),
            //]);

            return completion.ToString();

        } 
    }
            
}
