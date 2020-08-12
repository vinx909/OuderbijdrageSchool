using DotLiquid.Tags;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OuderbijdrageSchool
{
    public partial class Form1 : Form
    {
        //start 12:25
        private const int startingContribution = 50;
        private const int youngChildContribution = 25;
        private const int youngChildUpperLimit = 3;
        private const int oldChildContribution = 37;
        private const int oldChildUpperLimit = 2;
        private const int agdOfOldChild = 10;
        private const int singleParentPersentageReduction = 25;

        private const string currentDatelabelText = "current date:";
        private const string addChildButtonText = "add child";

        private const int widthMargin = 10;
        private const int heightMargin = 10;
        private const int rowHeight = 30;

        private Date currentDate;
        private List<ChildBirthDate> childBirthDates;
        private Button addChildButton;

        public Form1()
        {
            InitializeComponent();
            CurrentDateInitialize();
            ChildBirthDatesInitialize();
            AddChildButtonInitialize();
            ResetPositions();
        }

        private void CurrentDateInitialize()
        {
            currentDate = new Date(this, currentDatelabelText);
        }

        private void ChildBirthDatesInitialize()
        {
            childBirthDates = new List<ChildBirthDate>();
            ChildBirthDatesAddNew();
        }
        private void ChildBirthDatesAddNew()
        {
            childBirthDates.Add(new ChildBirthDate(this));
            
        }
        private void AddChildButtonInitialize()
        {
            addChildButton = new Button();
            addChildButton.Text = addChildButtonText;
            addChildButton.Click += new EventHandler(ButtonFunctionAddChild);
            Controls.Add(addChildButton);
        }
        internal void ChildBirthDatesRemove(ChildBirthDate toRemove)
        {
            childBirthDates.Remove(toRemove);
            ResetPositions();
        }
        private int CalculateParentalContribution()
        {
            int total = startingContribution;
            int numberOfYoungerChilderen = 0;
            int numberOfOlderChilderen = 0;
            int[] currentDate = this.currentDate.GetDate();
            int yearIndex = Date.GetYearIndex();
            int monthIndex = Date.GetMonthIndex();
            int dayIndex = Date.GetDayIndex();
            foreach (ChildBirthDate childBirthDate in childBirthDates)
            {
                int[] birthDate = childBirthDate.GetDate();
                if (birthDate[yearIndex]+ agdOfOldChild<currentDate[yearIndex]||(birthDate[yearIndex] + agdOfOldChild <= currentDate[yearIndex]&&birthDate[monthIndex]<currentDate[monthIndex])|| (birthDate[yearIndex] + agdOfOldChild <= currentDate[yearIndex] && birthDate[monthIndex] <= currentDate[monthIndex] && birthDate[dayIndex] <= currentDate[dayIndex]))
                {
                    numberOfOlderChilderen++;
                }
                else
                {
                    numberOfYoungerChilderen++;
                }
            }
            total += Math.Min(numberOfYoungerChilderen, youngChildUpperLimit) * youngChildContribution;
            total += Math.Min(numberOfOlderChilderen, oldChildUpperLimit) * oldChildContribution;
            return total;
        }

        private void ResetPositions()
        {
            int numberOfRowsDown = 0;
            currentDate.ChangePosition(widthMargin, heightMargin + rowHeight * numberOfRowsDown);
            numberOfRowsDown++;
            foreach (ChildBirthDate childBirthDate in childBirthDates)
            {
                childBirthDate.ChangePosition(widthMargin, heightMargin + rowHeight * numberOfRowsDown);
                numberOfRowsDown++;
            }
            addChildButton.Location = new Point(widthMargin, heightMargin + rowHeight * numberOfRowsDown);
            numberOfRowsDown++;
        }

        private void ButtonFunctionAddChild(object sender, EventArgs e)
        {
            ChildBirthDatesAddNew();
            ResetPositions();
        }

        internal class Date
        {
            protected const int textBoxOfset = 100;
            protected const int textBoxBetweenOfset = 30;
            protected const int textBoxWidth = 25;

            protected const int dateArrayLength = 3;
            protected const int dateDayIndex = 0;
            protected const int dateMonthIndex = 1;
            protected const int dateYearIndex = 2;

            protected Label label;
            protected TextBox textBoxDay;
            protected TextBox textBoxMonth;
            protected TextBox textBoxYear;

            protected Form1 form;

            internal Date(Form1 form, string labelText)
            {
                this.form = form;

                label = new Label();
                label.Text = labelText;
                form.Controls.Add(label);

                textBoxDay = new TextBox();
                textBoxDay.Width = textBoxWidth;
                form.Controls.Add(textBoxDay);

                textBoxMonth = new TextBox();
                textBoxMonth.Width = textBoxWidth;
                form.Controls.Add(textBoxMonth);

                textBoxYear = new TextBox();
                textBoxYear.Width = textBoxWidth;
                form.Controls.Add(textBoxYear);
            }

            internal void ChangePosition(int widthOfset, int heightOfset)
            {
                CorrectPosition(widthOfset, heightOfset);
            }
            protected void CorrectPosition(int widthOfset, int heightOfset)
            {
                label.Location = new Point(widthOfset, heightOfset);
                textBoxDay.Location = new Point(widthOfset + textBoxOfset, heightOfset);
                textBoxMonth.Location = new Point(widthOfset + textBoxOfset + textBoxBetweenOfset, heightOfset);
                textBoxYear.Location = new Point(widthOfset + textBoxOfset + textBoxBetweenOfset * 2, heightOfset);
            }
            internal int[] GetDate()
            {
                int[] date = new int[dateArrayLength];
                date[dateDayIndex] = int.Parse(textBoxDay.Text);
                date[dateMonthIndex] = int.Parse(textBoxMonth.Text);
                date[dateYearIndex] = int.Parse(textBoxYear.Text);
                return date;
            }
            internal static int GetYearIndex()
            {
                return dateYearIndex;
            }
            internal static int GetMonthIndex()
            {
                return dateMonthIndex;
            }
            internal static int GetDayIndex()
            {
                return dateDayIndex;
            }
        }
        internal class ChildBirthDate : Date
        {
            private const string labelText = "date of childs birth (day-month-year):";
            private const string buttonText = "remove";
            
            private Button button;

            internal ChildBirthDate(Form1 form) :base(form, labelText)
            {
                label.Text = labelText;

                button = new Button();
                button.Text = buttonText;
                button.Click += new EventHandler(ButtonFunction);
                form.Controls.Add(button);
            }
            private void ButtonFunction(object sender, EventArgs e)
            {
                form.Controls.Remove(label);
                form.Controls.Remove(textBoxDay);
                form.Controls.Remove(textBoxMonth);
                form.Controls.Remove(textBoxYear);
                form.Controls.Remove(button);
                form.ChildBirthDatesRemove(this);
            }
            internal void ChangePosition(int widthOfset, int heightOfset)
            {
                CorrectPosition(widthOfset, heightOfset);
                button.Location = new Point(widthOfset + textBoxOfset + textBoxBetweenOfset * 3, heightOfset);
            }
        }
    }
}
