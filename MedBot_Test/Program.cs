using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Timers;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace MedBot_Test
{

    class Program
    {
        public const String start_id = "start";
        public const String token = "1685573207:AAEVEOsUTRVt_ciJgCwGDKEmfHiRcL-6egY";
        private static ITelegramBotClient Bot;
        static long chatId = 0;
        static String parentBtnID = start_id;
        static List<Comand> data;
        static bool isHelpMode = false;
        static Timer updateTimer;
        static ulong LastID = 0;
        static DateTime datetimenow;

        static void Main(string[] args)
        {
            DBClass.connect("D:/DB/Debug.db");
            Bot = new TelegramBotClient(token);
            datetimenow = new DateTime();
            var me = Bot.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);
            updateTimer = new Timer();
            updateTimer.Interval = 100;
            updateTimer.Elapsed += messUpdate;

            Bot.OnMessage += HandleMessage;
            Bot.OnCallbackQuery += makeChoice;

            Bot.StartReceiving();
            while (true) ;
        }

        async static void HandleMessage(object sender, MessageEventArgs message)
        {
            chatId = message.Message.Chat.Id;
            parentBtnID = start_id;
            var text = message.Message.Text;
            datetimenow = DateTime.Now;
            if (!isHelpMode)
            {
                await Bot.SendTextMessageAsync(chatId, "Здравствуйте! Это медицинский бот и он может помочь Вам с возникшими вопросами.");
                showMessage();
                DBClass.clearQuery(chatId.ToString());
            }
            else
            {
                if(text == "/end" || text == "Завершить сеанс")
                {
                    DBClass.clearQuery(chatId.ToString());
                    stopHelp();
                }
                else DBClass.putMessage(text, chatId.ToString());
            }
        }

        static void makeChoice(object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev)
        {
            if (data == null) return;
            var message = ev.CallbackQuery.Data;
            try
            {
                String[] messArr = message.Split("_");
                if (messArr[0] != parentBtnID) return;
                if (messArr[1] == "back")
                {
                    Comand t = DBClass.getComand(messArr[0]);
                    parentBtnID = t.ParentID;
                    showMessage();
                }
                else if (messArr[1] == "helpyes")
                {
                    workWithHelp(true);
                }
                else if (messArr[1] == "helpno")
                {
                    Comand t = DBClass.getComand(messArr[0]);
                    parentBtnID = t.ParentID;
                    showMessage();
                }
                else
                {
                    parentBtnID = data[Convert.ToInt32(messArr[1])].Id;
                    showMessage();
                }
            }
            catch(Exception) { }
        }

        async static void showMessage()
        {
            data = DBClass.getComands(parentBtnID);
            if (data.Count == 0)
            {
                await Bot.SendTextMessageAsync(chatId, "Упс! Тут пока ничего нет. Мы вернём Вас назад)");
                parentBtnID = DBClass.getComand(parentBtnID).ParentID;
                showMessage();
            }
            else if (data.Count > 0 && data[0].Type == "1")
            {
                List<List<InlineKeyboardButton>> btns = new List<List<InlineKeyboardButton>>();

                for (int i = 0; i < data.Count; i++)
                {
                    List<InlineKeyboardButton> lst = new List<InlineKeyboardButton>();
                    lst.Add(InlineKeyboardButton.WithCallbackData(data[i].Text, parentBtnID + "_" + i));
                    btns.Add(lst);
                }
                if (parentBtnID != start_id)
                {
                    List<InlineKeyboardButton> back = new List<InlineKeyboardButton>();
                    back.Add(InlineKeyboardButton.WithCallbackData("Назад", parentBtnID + "_back"));
                    btns.Add(back);
                }
                InlineKeyboardMarkup keybrd = new InlineKeyboardMarkup(btns);
                await Bot.SendTextMessageAsync(chatId, "Выберите один из пунктов: ", replyMarkup: keybrd);
            }
            else if (data.Count == 1 && data[0].Type == "2")
            {
                InlineKeyboardMarkup keybrd = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Назад", parentBtnID + "_back"));
                await Bot.SendTextMessageAsync(chatId, data[0].Text, replyMarkup: keybrd);
            }
            else if (data.Count == 1 && data[0].Type == "3")
            {
                await Bot.SendTextMessageAsync(chatId, data[0].Text);
                workWithHelp();
            }
        }

        async static void messUpdate(object sender, ElapsedEventArgs args)
        {
            if (DBClass.checkQuery(chatId.ToString()) == 0) datetimenow = DateTime.Now;
            else if (DBClass.checkQuery(chatId.ToString()) == -1) {
                stopHelp();
            } 
            else if (DateTime.Now.Subtract(datetimenow).TotalMinutes > 10){
                await Bot.SendTextMessageAsync(chatId, "Вы бездействовали дольше 10 минут и были отключены!");
                stopHelp();
                DBClass.clearQuery(chatId.ToString());
            }
            List<ChatMess> messes = DBClass.getMessages(chatId.ToString(), LastID.ToString());
            if (messes.Count > 0)
            {
                LastID = Convert.ToUInt64(messes[messes.Count - 1].Id.ToString());
                foreach (ChatMess chatmess in messes)
                {
                    await Bot.SendTextMessageAsync(chatId, chatmess.Mesage);
                }
            }
        }

        async static void stopHelp()
        {
            if (!isHelpMode) return;
            isHelpMode = false;
            updateTimer.Stop();
            await Bot.SendTextMessageAsync(chatId, "Сеанс завершён!");
        }

        static void workWithHelp(bool pass = false)
        {
            if (!pass)
            {
                List<List<InlineKeyboardButton>> btns = new List<List<InlineKeyboardButton>>();

                List<InlineKeyboardButton> yes = new List<InlineKeyboardButton>();
                yes.Add(InlineKeyboardButton.WithCallbackData("Да", parentBtnID + "_helpyes"));

                List<InlineKeyboardButton> no = new List<InlineKeyboardButton>();
                no.Add(InlineKeyboardButton.WithCallbackData("Нет", parentBtnID + "_helpno"));

                btns.Add(yes); btns.Add(no);
                InlineKeyboardMarkup keybrd = new InlineKeyboardMarkup(btns);
                Bot.SendTextMessageAsync(chatId, "Хотите связаться со специалистом?", replyMarkup: keybrd);
            }
            else
            {
                String mess = "Сейчас вы будете связаны со специалистом. Это может занять некоторое время";
                LastID = DBClass.getLastID(chatId.ToString());
                var markup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("Завершить сеанс"),
                });
                markup.ResizeKeyboard = true;
                markup.OneTimeKeyboard = true;
                Bot.SendTextMessageAsync(chatId, mess, replyMarkup: markup);
                isHelpMode = true;
                updateTimer.Start();
                datetimenow = DateTime.Now;
                DBClass.putQuery(chatId.ToString());
            }
        }
    }
}
