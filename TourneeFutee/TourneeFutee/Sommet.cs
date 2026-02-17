using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourneeFutee
{
    internal class Sommet
    {
        private string nom;
        private int valeur;
        private int indice;

        public Sommet(string nom, int valeur, int indice)
        {
            this.nom = nom;
            this.valeur = valeur;
            this.indice = indice;
        }

        public string Nom
        {
            get { return nom; }
        }
        public int Valeur
        {
            get { return valeur; }
        }
        public int Indice
        {
            get { return indice; }
            set { indice = value; }
        }

    }
}
