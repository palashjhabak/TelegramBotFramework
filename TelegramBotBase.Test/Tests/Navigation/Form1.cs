﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBotBase.Base;
using TelegramBotBase.Form;

namespace TelegramBotBaseTest.Tests.Navigation
{
    public class Form1 : FormBase
    {
        Message msg = null;

        public Form1()
        {
            this.Closed += Form1_Closed;
        }

        private async Task Form1_Closed(object sender, EventArgs e)
        {
            if (msg == null)
                return;

            await Device.DeleteMessage(msg);
        }

        public override async Task Action(MessageResult message)
        {
            if (message.Handled)
                return;

            await message.ConfirmAction();

            switch (message.RawData)
            {
                case "next":

                    message.Handled = true;

                    var f1 = new Form1();

                    await NavigationController.PushAsync(f1);


                    break;

                case "previous":

                    message.Handled = true;

                    //Pop's the current form and move the previous one. The root form will be the Start class.
                    await NavigationController.PopAsync();

                    break;

                case "root":

                    message.Handled = true;

                    await NavigationController.PopToRootAsync();

                    break;


            }



        }

        public override async Task Render(MessageResult message)
        {
            if (msg != null)
                return;

            var bf = new ButtonForm();
            bf.AddButtonRow("Next page", "next");
            bf.AddButtonRow("Previous page", "previous");
            bf.AddButtonRow("Back to root", "root");

            msg = await Device.Send($"Choose your options (Count on stack {NavigationController.Index + 1})", bf);

        }


    }
}
