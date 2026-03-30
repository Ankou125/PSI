namespace TourneeFutee
{
    // Modélise une tournée dans le cadre du problème du voyageur de commerce
    public class Tour
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 

        // propriétés

        // Coût total de la tournée
        public float Cost
        {
            get;    // TODO : implémenter
        }

        // Nombre de trajets dans la tournée
        public int NbSegments
        {
            get;    // TODO : implémenter
        }


        // Renvoie vrai si la tournée contient le trajet `source`->`destination`
        public bool ContainsSegment((string source, string destination) segment)
        {
            return false;   // TODO : implémenter 
        }


        // Affiche les informations sur la tournée : coût total et trajets
        public void Print()
        {
            // TODO : implémenter 
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }
}
