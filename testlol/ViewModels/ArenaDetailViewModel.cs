using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using testlol.API;
using testlol.Models.DTOs.Match_V5;

namespace testlol.ViewModels
{
    internal class ArenaDetailViewModel : ViewModelBase
    {

        public ArenaDetailViewModel(Dictionary<int, List<ParticipantDTO>> arena, long gametime)
        {
            GameDuration = gametime;
            ArenaTeam = arena;
            LoadMembers();
        }
        public ArenaDetailViewModel()
        {

        }

        private Dictionary<int, List<ParticipantDTO>> arenaTeam;
        public Dictionary<int, List<ParticipantDTO>> ArenaTeam
        {
            get => arenaTeam;
            set => SetProperty(ref arenaTeam, value);
        }

        private long gameduration;
        public long GameDuration
        {
            get => gameduration;
            set => SetProperty(ref gameduration, value);
        }

        private ObservableCollection<DetailListItemViewModel> innermembers
        { get; } = new ObservableCollection<DetailListItemViewModel>();

        private CollectionViewSource groupedMembers;
        public CollectionViewSource GroupedMembers
        {
            get
            {
                if (groupedMembers == null && innermembers !=null)
                {
                    groupedMembers = new CollectionViewSource
                    {
                        Source = innermembers
                    };
                    groupedMembers.View.GroupDescriptions.Add(new PropertyGroupDescription("Ranking"));
                }
                return groupedMembers;
            }
        }

        private void LoadMembers()
        {
            if (ArenaTeam == null)
                return;

            // MaxDamge와 MaxDamgeTaken 값을 미리 계산하여 전달하도록 수정
            int maxDamge = 0;
            int maxDamgeTaken = 0;
            foreach (var participants in ArenaTeam.Values)
            {
                foreach (var value in participants)
                {
                    if (maxDamge <= value.totalDamageDealtToChampions)
                        maxDamge = value.totalDamageDealtToChampions;

                    if (maxDamgeTaken <= value.totalDamageTaken)
                        maxDamgeTaken = value.totalDamageTaken;
                }
            }

            List<DetailListItemViewModel> sortedList = new List<DetailListItemViewModel>();
            foreach (var teamId in ArenaTeam.Keys)
            {
                foreach (var participant in ArenaTeam[teamId])
                {
                    sortedList.Add(DetailListItemViewModel.From(GameDuration, maxDamge, maxDamgeTaken, participant));
                }
            }

            foreach (var item in sortedList.OrderBy(x => x.Ranking).Where(x => x != null))
            {
                innermembers.Add(item);
            }
        }


    }
}
