using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace answersbot.Resources
{
    public class Strings
    {
        public static string FirstMessage = "Bem vindo! Você pode perguntar ou responder anonimamente, o que deseja ?";
        public static string SendQuestion = "Envie sua pergunta";

        public static string ResetMessageByAnswer = "Resposta enviada (emoticon)! " + RestartingMessage;
        public static string ResetMessageByQuestion = "Sua pergunta foi registrada (emoticon)! Em breve você receberá respostas. \n" + RestartingMessage;

        public static string RestartingMessage = "Para perguntar ou responder novamente envie COMEÇAR";

        public static string KeepAnsweringActionText = "Continuar respondendo";
        public static string QuestionClosedActionText = "Pergunta respondida";
        public static string SendAnotherQuestionActionText = "Quero outra pergunta";
        public static string QuestionActionText = "1 - Perguntar";
        public static string AnswerActionText = "2 - Responder";
        public static string StartActionText = "Começar";
    }
}