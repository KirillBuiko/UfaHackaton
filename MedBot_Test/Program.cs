using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Timers;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;

namespace MedBot_Test
{

    class Program
    {
        public const String start_id = "start";
        public const String token = "1685573207:AAEVEOsUTRVt_ciJgCwGDKEmfHiRcL-6egY";
        private static ITelegramBotClient Bot;
        static List<Comand> data;
        static bool isHelpMode = false;
        static Timer updateTimer;
        static ulong LastID = 0;
        static DateTime datetimenow;
        static SortedDictionary<long, string> Users = new SortedDictionary<long, string>();

        static void Main(string[] args)
        {
            Program pr = new Program();
            pr.Init();
        }

        public void Init()
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
            var chatID = message.Message.Chat.Id;
            if (!Users.ContainsKey(chatID)) Users.Add(chatID, start_id);
            else Users[chatID] = start_id;
            var text = message.Message.Text;
            datetimenow = DateTime.Now;
            if (!isHelpMode)
            {
                await Bot.SendTextMessageAsync(chatID, "Здравствуйте! Это медицинский бот и он может помочь Вам с возникшими вопросами.");
                showMessage(chatID);
                DBClass.clearQuery(chatID.ToString());
            }
            else
            {
                if(text == "/end" || text == "Завершить сеанс")
                {
                    DBClass.clearQuery(chatID.ToString());
                    stopHelp(chatID);
                }
                else DBClass.putMessage(text, chatID.ToString());
            }
        }

        static void makeChoice(object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev)
        {
            if (data == null) return;
            var message = ev.CallbackQuery.Data;
            Console.WriteLine(message);
            var chatID = ev.CallbackQuery.Message.Chat.Id;
            data = DBClass.getComands(Users[chatID]);
            try
            {
                String[] messArr = message.Split("_");
                if (messArr[0] != Users[chatID]) 
                    return;
                if (messArr[1] == "back")
                {
                    Comand t = DBClass.getComand(messArr[0]);
                    Users[chatID] = t.ParentID;
                    showMessage(chatID);
                }
                else if (messArr[1] == "helpyes")
                {
                    workWithHelp(chatID, true);
                }
                else if (messArr[1] == "helpno")
                {
                    Comand t = DBClass.getComand(messArr[0]);
                    Users[chatID] = t.ParentID;
                    showMessage(chatID);
                }
                else
                {
                    Users[chatID] = data[Convert.ToInt32(messArr[1])].Id;
                    showMessage(chatID);
                }
            }
            catch(Exception) { }
        }

        async static void showMessage(long chatID)
        {
            data = DBClass.getComands(Users[chatID]);
            foreach (var bla in Users)
                Console.Out.WriteLine(bla);
            if (data.Count == 0)
            {
                await Bot.SendTextMessageAsync(chatID, "Упс! Тут пока ничего нет. Мы вернём Вас назад)");
                Users[chatID] = DBClass.getComand(Users[chatID]).ParentID;
                showMessage(chatID);
            }
            else if (data.Count > 0 && data[0].Type == "1")
            {
                List<List<InlineKeyboardButton>> btns = new List<List<InlineKeyboardButton>>();

                for (int i = 0; i < data.Count; i++)
                {
                    List<InlineKeyboardButton> lst = new List<InlineKeyboardButton>();
                    lst.Add(InlineKeyboardButton.WithCallbackData(data[i].Text, Users[chatID] + "_" + i.ToString()));
                    btns.Add(lst);
                }
                if (Users[chatID] != start_id)
                {
                    List<InlineKeyboardButton> back = new List<InlineKeyboardButton>();
                    back.Add(InlineKeyboardButton.WithCallbackData("Назад", Users[chatID] + "_back"));
                    btns.Add(back);
                }
                InlineKeyboardMarkup keybrd = new InlineKeyboardMarkup(btns);
                await Bot.SendTextMessageAsync(chatID, "Выберите один из пунктов: ", replyMarkup: keybrd);
            }
            else if (data.Count == 1 && data[0].Type == "2")
            {
                InlineKeyboardMarkup keybrd = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Назад", Users[chatID] + "_back"));
                await Bot.SendTextMessageAsync(chatID, data[0].Text, replyMarkup: keybrd);
            }
            else if (data.Count == 1 && data[0].Type == "3")
            {
                await Bot.SendTextMessageAsync(chatID, data[0].Text);
                workWithHelp(chatID);
            }
        }

        async static void messUpdate(object sender, ElapsedEventArgs args)
        {
            foreach (var user in Users) {
                var chatID = user.Key;
                if (DBClass.checkQuery(chatID.ToString()) == 0) datetimenow = DateTime.Now;
                else if (DBClass.checkQuery(chatID.ToString()) == -1) {
                    stopHelp(chatID);
                }
                else if (DateTime.Now.Subtract(datetimenow).TotalMinutes > 10) {
                    await Bot.SendTextMessageAsync(chatID, "Вы бездействовали дольше 10 минут и были отключены!");
                    stopHelp(chatID);
                    DBClass.clearQuery(chatID.ToString());
                }
                List<ChatMess> messes = DBClass.getMessages(chatID.ToString(), LastID.ToString());
                if (messes.Count > 0)
                {
                    LastID = Convert.ToUInt64(messes[messes.Count - 1].Id.ToString());
                    foreach (ChatMess chatmess in messes)
                    {
                        await Bot.SendTextMessageAsync(chatID, chatmess.Mesage);
                    }
                }
            }
        }

        async static void stopHelp(long chatID)
        {
            if (!isHelpMode) return;
            isHelpMode = false;
            updateTimer.Stop();
            await Bot.SendTextMessageAsync(chatID, "Сеанс завершён!");
        }

        static void workWithHelp(long chatID, bool pass = false)
        {
            if (!pass)
            {
                List<List<InlineKeyboardButton>> btns = new List<List<InlineKeyboardButton>>();

                List<InlineKeyboardButton> yes = new List<InlineKeyboardButton>();
                yes.Add(InlineKeyboardButton.WithCallbackData("Да", Users[chatID] + "_helpyes"));

                List<InlineKeyboardButton> no = new List<InlineKeyboardButton>();
                no.Add(InlineKeyboardButton.WithCallbackData("Нет", Users[chatID] + "_helpno"));

                btns.Add(yes); btns.Add(no);
                InlineKeyboardMarkup keybrd = new InlineKeyboardMarkup(btns);
                Bot.SendTextMessageAsync(chatID, "Хотите связаться со специалистом?", replyMarkup: keybrd);
            }
            else
            {
                String mess = "Сейчас вы будете связаны со специалистом. Это может занять некоторое время";
                LastID = DBClass.getLastID(chatID.ToString());
                var markup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("Завершить сеанс"),
                });
                markup.ResizeKeyboard = true;
                markup.OneTimeKeyboard = true;
                Bot.SendTextMessageAsync(chatID, mess, replyMarkup: markup);
                isHelpMode = true;
                updateTimer.Start();
                datetimenow = DateTime.Now;
                DBClass.putQuery(chatID.ToString());
            }
        }
    }
}
