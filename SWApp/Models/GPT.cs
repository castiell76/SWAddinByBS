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
            string apiKey = "";
            ChatClient client = new(model: "gpt-3.5-turbo", apiKey);
            ChatCompletion completion = client.CompleteChat($"Przetłumacz wyrażenie: {phraseToTranslate}");

            return completion.ToString();

        } 
    }
            
}
