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

        private ReadOnlyObservableCollection<DetailListItemViewModel> members;
        public ReadOnlyObservableCollection<DetailListItemViewModel> Members
        {
            get
            {

                if (members == null && ArenaTeam != null)
                {
                    members = new ReadOnlyObservableCollection<DetailListItemViewModel>(innermembers);
                }
                return members;
            }
        }
        private void LoadMembers()
        {
            if (ArenaTeam == null)
                return;

            Match_V5 match_V5 = new Match_V5();
            int maxDamge = match_V5.GetArenaMaxDamge(ArenaTeam);
            int maxDamgeTaken = match_V5.GetArenaMaxDamgeTaken(ArenaTeam);

            // 데이터를 정렬해서 추가하기 위해 임시 리스트를 생성하고 팀 ID 기준으로 정렬
            List<DetailListItemViewModel> sortedList = new List<DetailListItemViewModel>();
            foreach (var teamId in ArenaTeam.Keys)
            {
                foreach (var participant in ArenaTeam[teamId])
                {
                    participant.placement = teamId;
                    sortedList.Add(DetailListItemViewModel.From(match_V5, GameDuration, maxDamge, maxDamgeTaken, participant));
                }
            }

            // 정렬된 데이터를 innermembers에 추가
            foreach (var item in sortedList.OrderBy(x => x.Ranking))
            {
                innermembers.Add(item);
            }

        }
    }
}
