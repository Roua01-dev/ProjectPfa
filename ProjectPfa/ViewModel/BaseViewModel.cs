using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;


//mécanisme de notification des changements de propriétés

namespace ProjectPfa.ViewModel
{
    public class BaseViewModel: ObservableObject
    {
        //signaler que la valeur d'une propriété a changé
        //PropertyChanged est utilisé pour notifier la vue que les données de l'objet ont changé, permettant ainsi à l'interface utilisateur de se mettre à jour automatiquement
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")

        //automatiquement le nom de la propriété qui appelle la méthode OnPropertyChanged
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
