using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using FinnishApp.Models;

namespace FinnishApp.ViewModels
{
    public class InflectionsWindowViewModel : ReactiveObject
    {
        public string Title { get; }
        public ObservableCollection<InflectionRow> Rows { get; }

        public InflectionsWindowViewModel(WordPair pair)
        {
            Title = $"Odmiany: {pair.Finnish}";

            // Zakładamy, że klucze w obu słownikach są identyczne
            var keys = pair.InflectionsSingular.Keys
                .Intersect(pair.InflectionsPlural.Keys);

            Rows = new ObservableCollection<InflectionRow>(
                keys.Select(k => new InflectionRow {
                    CaseName = k,
                    Singular = pair.InflectionsSingular[k],
                    Plural   = pair.InflectionsPlural[k]
                })
            );
        }
    }
}