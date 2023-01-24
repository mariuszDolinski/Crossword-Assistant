using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Krzyżówka
{
    class Wzorzec
    {
        Label numer;
         MaskedTextBox litera;

        public Label Numer
        {
            get { return numer; }
            set { numer = value; }
        }

        public MaskedTextBox Litera
        {
            get { return litera; }
            set { litera = value; }
        }

        public Wzorzec(Form f)
        {
            inicjujWzorzec(f);
        }

        public void inicjujWzorzec(Form f)
        {
            numer = new Label();
            litera = new MaskedTextBox();

            litera.Location = new Point(5, 5);
            numer.Location = new Point(5, 35);

            litera.Width = 20;
            numer.Size = new Size(20, 15);

            litera.Mask = "a";
            numer.TextAlign = ContentAlignment.MiddleCenter;
            numer.Text = "1";
            numer.BorderStyle = BorderStyle.FixedSingle;

            f.Controls.Add(litera);
            f.Controls.Add(numer);
        }
    }
}
