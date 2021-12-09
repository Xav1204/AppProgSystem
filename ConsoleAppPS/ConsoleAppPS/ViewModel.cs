using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppPS
{
    class ViewModel : Model
    {
        public void demarrage()
        {
            //Sélection de la langue
            Console.Write("Choose your language / Choisir votre langue : ");
            var langue = Console.ReadLine();

            Langue(langue);

            var etat = true;

            // tant qu'on a pas cliqué sur Quitter
            while (etat == true)
            {
                debut();

                switch (Console.ReadLine())
                {
                    case "a":
                        Console.Write("{0}", langueNom);
                        var NameSave = Console.ReadLine();
                        Console.Write("{0}", langueSource);
                        var SourceSave = Console.ReadLine();
                        Console.Write("{0}", langueCible);
                        var TargetSave = Console.ReadLine();
                        Console.Write("{0}", langueType);
                        var TypeSave = Console.ReadLine();

                        Create(NameSave, SourceSave, TargetSave, TypeSave);
                        break;
                    case "m":
                        Modify();
                        break;
                    case "d":
                        Delete();
                        break;
                    case "r":
                        read();
                        break;
                    case "s":
                        Console.Write("{0}", langueNom);
                        var ChoixNom = Console.ReadLine();
                        Save(ChoixNom);
                        break;
                    case "p":
                        SequentialSave();
                        break;
                    case "q":
                        etat = false;
                        break;
                }

            }
            fin();
        }
    }
}
            
        


