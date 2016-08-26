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

        public static string RestartingMessage = "Para perguntar ou responder novamente envie COMEÇAR";
        public static string ResetMessageByAnswer = "Resposta enviada (emoticon)! " + RestartingMessage;
        public static string ResetMessageByQuestion = "Sua pergunta foi registrada (emoticon)! Em breve você receberá respostas. \n" + RestartingMessage;

        public static string FallbackMessage = "Desculpe não entendi! " + RestartingMessage;

        public static string KeepAnsweringActionText = "Continuar respondendo";
        public static string QuestionClosedActionText = "Pergunta respondida";
        public static string SendAnotherQuestionActionText = "Quero outra pergunta";
        public static string QuestionActionText = "Perguntar";
        public static string[] QuestionActionValues = { "perguntar", "pergunta", "p", "/perguntar", "/pergunta", "p", "1" };
        public static string AnswerActionText = "Responder";
        public static string[] AnswerActionValues = { "responder", "resposta", "r", "/responder", "/responsta", "r", "2" };
        public static string StartActionText = "Começar";

        public static string SkipQuestionActionText = "outra";
        public static string[] SkipQuestionActionValues = { "pular", "outra", "outra pergunta", "quero outra pergunta" };
        public static string NoQuestionsAvailable = "Parabéns! Você respondeu todas as perguntas disponíveis!";
        public static string KeepAnsweringActionValue = "continuar";
        public static string QuestionClosedActionValue = "encerrar";
    }
}