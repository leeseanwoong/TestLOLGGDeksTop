# WPF OP.GG

WPF OP.GG는 WPF(Windows Presentation Foundation)를 공부하기 위해 개발된 리그오브레전드 전적 검색 프로그램입니다.
이 프로젝트는 C# WPF를 사용하여 개발되었으며, Riot API와 LCU API를 이용하여 리그오브레전드의 사용자 정보와 전적 데이터를 받아와 출력합니다.

## 사용한 기술

- C#
- Windows Presentation Foundation (WPF)
- Riot API
- LCU API

## 주요 기능

- **자동 로그인 연동**
  - 사용자가 프로그램을 실행하면, 리그오브레전드를 로그인 상태로 자동으로 연동합니다.

- **사용자 정보 출력**
  - LCU API를 이용하여 연동한 사용자의 정보를 받아와서 화면에 출력합니다.
  - 사용자의 소환사명, 레벨, 티어 등을 표시합니다.

- **사용자의 전적 출력**
  - 사용자의 전적을 LCU API를 통해 조회하여 출력합니다.
  - 최근 전적, 승률, KDA 등의 정보를 제공합니다.
  - 전적에 대해 자세한 정보를 제공합니다.

- **다른 유저의 전적 검색 및 출력**
  - 다른 유저의 소환사 이름을 입력하여 Riot API를 통해 조회하여 출력합니다.
  - 최근 전적 및 해당 전적들의 자세한 정보를 제공합니다.

## 결과 화면

메인화면

<img width="80%" src="https://github.com/leeseanwoong/TestLOLGGDeksTop/assets/78595475/29359be3-4c18-4b70-bddf-d51a892f614c"/>

사용자 정보확인 화면(HomeView)

<img width="80%" src="https://github.com/leeseanwoong/TestLOLGGDeksTop/assets/78595475/8b48c7e3-4aaf-4686-b7c1-dd61cd268db3"/>

사용자 전적확인 화면(RecordView)

<img width="80%" src="https://github.com/leeseanwoong/TestLOLGGDeksTop/assets/78595475/10c476d1-6782-462f-b99c-0a3474e7c298"/>

사용자 전적 자세한 정보확인 화면(DetailRecordView)

<img width="80%"  src="https://github.com/leeseanwoong/TestLOLGGDeksTop/assets/78595475/3a61a0c2-f45c-465b-a758-a01bb35f8710"/>

아레나 모드 전적 자세한 정보확인 화면(ArenaDetailView)

<img width="80%"  src="https://github.com/leeseanwoong/TestLOLGGDeksTop/assets/78595475/45257a08-4fc2-4424-ad98-539dcd1977ac"/>

선택한 전적에서 사용한 룬 확인 화면(PerksPopUpView)

<img width="80%" src="https://github.com/leeseanwoong/TestLOLGGDeksTop/assets/78595475/82c28457-ae44-45f3-babf-d976893ef10f"/>

다른 사용자 검색 화면(SearchView)

<img width="80%" src="https://github.com/leeseanwoong/TestLOLGGDeksTop/assets/78595475/25fe2236-52ad-486b-aa20-f926384aa5cc"/>


