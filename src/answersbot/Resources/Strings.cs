﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace answersbot.Resources
{
    public class Strings
    {
        public static string FirstMessage = "Bem-vindx 🙂 \n\nComigo você pode perguntar ou responder anonimamente, o que deseja?";
        public static string SendQuestion = "Envie sua pergunta em apenas uma mensagem 📩 \n\n⚠️ considero cada 📩 como uma pergunta";

        public static string RestartingMessage = "Deseja perguntar ou responder novamente?";
        public static string ResetMessageByClosing = "Sua pergunta '{0}' foi encerrada. :)\n" + RestartingMessage;
        public static string ResetMessageByAnswer = "Resposta enviada :)\n" + RestartingMessage;
        public static string ResetMessageByQuestion = "Sua pergunta foi registrada. :)\nEm breve você receberá respostas.\n" + RestartingMessage;

        public static string FallbackMessage = "Desculpe não entendi! " + RestartingMessage;

        public static string KeepAnsweringActionText = "❎ Não Respondida";
        public static string QuestionClosedActionText = "✅ Respondida";
        public static string SendAnotherQuestionActionText = "Quero outra pergunta";
        public static string QuestionActionText = "Perguntar";
        public static string[] QuestionActionValues = { "perguntar", "pergunta", "p", "/perguntar", "/pergunta", "p", "1" };
        public static string AnswerActionText = "Responder";
        public static string[] AnswerActionValues = { "responder", "resposta", "r", "/responder", "/responsta", "r", "2" };
        public static string[] StartActionValues = { "começar", "início", "inicio", "/start", "comecar", "oi", "iniciar" };

        public static string[] SkipQuestionActionValues = { "pular", "outra", "outra pergunta", "quero outra pergunta", "1" };
        public static string NoQuestionsAvailable = "Parabéns! Você respondeu todas as perguntas disponíveis!";
        public static string KeepAnsweringActionValue = "continuar";
        public static string QuestionClosedActionValue = "encerrar";
    }
}