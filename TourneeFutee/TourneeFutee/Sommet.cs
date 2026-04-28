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
        private uint id;

        public Sommet(string nom, float valeur)
        {
            this.nom = nom;
            this.valeur = valeur;
            this.indice = compteur;
            this.id = 0;
            compteur++;
        }
        public Sommet(string nom)
        {
            this.nom = nom;
            this.valeur = 0;
            this.indice = compteur;
            this.id = 0;
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
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
    }
}
