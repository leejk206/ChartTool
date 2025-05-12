For Dev

NormalNote, SlideNote는 JSON 파일 위에서 List<List<int>> 자료형으로 저장합니다

각 List는 List 자료형을 7개씩 갖고 있으며, 내부 리스트는 각 Line의 NoteData(Int)를 보유하고 있습니다.

각 Integer는 Mod 16값에 따라 위치가 정해집니다. 노래 시작 시 첫 번째 박자는 0, 1박 뒤 노트는 16, 2박 뒤 노트는 32 등으로 설정하였습니다.

FlickNote는 List<List<int, bool>> 자료형으로 제작할 예정입니다.

내부 리스트는 각 Line의 노트 데이터를 갖고 있고, Int 값은 NormalNote, SlideNote의 동일하며 bool값이 true일 경우 위로 슬라이드, false일 경우 아래로 슬라이드로 설정하겠습니다.

HoldNote의 경우 회의를 거친 이후 제작 예정입니다.



For Design

Save Chart 버튼을 누를 경우 파일이 저장되는 위치가 디버깅 로그로 뜨도록 하였습니다.

이 경로에 위치하는 MyChart.Json 파일이 현재 편집되는 채보 데이터입니다.

EditMode에서 KeyPad 1번을 누를 경우 해당 위치에 NormalNote가 그려지며, 현재까지는 NormalNote만을 구현하였습니다.

2번은 HoldNote, 3번은 SlideNote, 4번은 FlickNote로 구현하겠습니다.

NormalNote와 SlideNote는 토글 시 사라지도록 구현하고, FlickNote는 토글 시 위 슬라이드 -> 아래 슬라이드 -> 삭제 순으로 구현할 예정입니다.

PlayMode에서 Play를 누를 경우 현재까지 제작한 Note들이 BPM에 맞춰 떨어지며, BPM은 Hierarchy의 PlayController의 Inspector 화면에서 BPM을 설정할 수 있게 하였습니다.

기타 궁금한 점은 언제든지 제게 물어봐 주시길 바랍니다.
