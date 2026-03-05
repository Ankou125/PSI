using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourneeFutee
{
    public class Sommet
    {
        private static int compteur = 0;
        private string nom;
        private float valeur;
        private int indice;

        public Sommet(string nom, float valeur)
        {
            this.nom = nom;
            this.valeur = valeur;
            this.indice = compteur;
            compteur++;
        }

        public string Nom
        {
            get { return nom; }
        }
        public float Valeur
        {
            get { return valeur; }
            set { valeur = value; }
        }
        public int Indice
        {
            get { return indice; }
            set { indice = value; }
        }

    }
}
