# Multi_Shooting_Game

# Resource Site

- https://www.kenney.nl/assets/category:2D
- https://resourcebank.or.kr/?menucode=30100&tmenu=graphics&mode=&catemenug=111&orderby=&s_type=&s_key=%ED%94%BD%EC%85%80
- https://opengameart.org/
- https://assetstore.unity.com/?category=2d%2Fcharacters&free=true&orderBy=1&page=3
- https://craftpix.net/download/51577/

# Information

- Unity Editor Ver : 2022.3.9f1
- IDE : VSCode
- Third Party
  - Photon 2
  - Firebase

# 작업 목록

- 무기

  - 플레이어 무기 칸에 상속됨 - check
  - 무기 획득하면 공격력 값 초기화 - check
  - 공격방식 override - check
  - 총알 공격 딜레이 구현 - check

- 플레이어

  - 총알 발사 - check

- 적

  - 총알 발사 - check
  - 넉백 - check

- 보스

  - 총알 발사 - check

# 코딩 컴벤션

- https://docs.popekim.com/ko/coding-standards/pocu-csharp

# 멀티 구현 리스트

1. 멀티플레이 클릭

- 매치메이킹 시도

  1. 서버에 방이 존재하는지 검색을 한다.
  2. 서버에 방이 존재하면 입장한다.
  3. 서버에 방이 1개도 없다면 room_1의 이름으로 방을 만든다.

- 방이 없으면 방 생성 후 플레이어 대기

2. 플레이어 입장

- 방의 현재 플레이어 2명 이상이 되는 경우
- 캐릭터 선택 씬으로 넘어가기
- 호스트, 게스트 모두 씬 전환

3. 캐릭터 선택 후 준비완료

- 2명의 플레이어 모두 준비완료
- 카운트 다운 시작 5초

4. 인 게임 입장

- 호스트방식의 서버로 구현
- 호스트와 게스트의 통신 구현
- 각 몬스터의 정보 동기화

5. 게임 셋

- 방의 삭제
- 각 정보들 초기화
- 서버에서 퇴장

# 개발일지

- 2023.09.13

  - 기획서 작성
  - 깃 허브 및 유니티 초기 셋팅
  - 초기화 씬에서 타이틀 화면으로 넘어가기 구현
  - 각종 설정들을 초기화 해주는 SettingManager 구현

- 2023.09.14

  - 유니티 파일 추가
  - TMP 셋업
  - 기본 폰트 추가 (메이플스토리 폰트)
  - 프로젝트 URP로 재 생성
  - Ingame씬 작업
    - input System 추가
    - 플레이어 인풋 추가
    - 마우스 인풋 추가
    - 마우스 좌클릭, 우클릭 추가
    - 마우스 위치에 따라 총구, 사람도 회전함
    - 플레이어 사격 구현
    - 적 총알 발사 구현
    - 적, 플레이어 피격시 사라짐 구현
  - LayerMask 설정 추가, Layer Collision Matrix 설정
  - 벽추가, 적 움직임 추가
  - 엔딩구현
    - 엔딩조건
      - 플레이어 사망
      - 적 사망
  - 엔딩 씬 구현
    - 데스크톱으로 나가기
    - 타이틀로 돌아가기

- 2023.09.15

  - 풀링 시스템 Static으로 구현
  - 전체 중력 x, y 전부 0으로 변경
  - rigidbody - Dynamic으로 변경
    - 콜라이더 충돌을 위함
  - 풀링 시스템 Dictionary로 변경
  - 몬스터 스포너 추가
  - 몬스터 풀링 추가
  - 플레이어, 적 체력 추가
  - 공격 당하는 인터페이스 추가
  - 다시 시작시 생기는 버그 발생
    - 풀매니저 오류
  - 프토로타입 완성
    - 몬스터 스폰, 몬스터 공격, 몬스터 이동 구현
    - 플레이어 공격, 플레이어 이동, 플레이어 체력, 플레이어 체력이 전부 닳게 되면 엔딩씬으로 이동
    - 오브젝트 풀링 Dictionary로 구현
      - Lisk of Rain 모작프로젝트 에서 참고
      - key : 풀이름(string)
      - value : 풀링할 오브젝트 (Stack<GameObject>)
    - 게임 다시하기 구현 -> 현재 발생한 버그 없음

- 2023.09.16

  - 노션에 UI 프로그래밍 패턴 추가
  - 폭탄 UI 초기 단계 셋업

- 2023.09.18

  - 작업할 요소들
    - 무기 - 1순위
      - 연사형, 방사형 등등
    - 디테일 - 2순위
      - 몬스터 넉백
    - 보스 - 3순위
      - 한명
        - 방사형으로 공격 - check
        - 적 스폰
    - 오브젝트 - 4순위
      - 폭탄 ( 탄막을 없애는 폭탄 ) - check
      - 장탄수 - check
    - 캐릭터 선택씬 - 5순위
      - 캐릭터 2명
    - 멀티 - 6순위
    - 카메라 연출, 사운드 - 7순위
  - 무기 2가지 추가
    - 머신건
    - 샷건

- 2023.09.19

  - 몬스터 넉백 추가
  - 몬스터 스폰시 몬스터 스탯 초기화 (Init)
  - 보스 - 코루틴을 시작할수 없는 버그
    - 게임 오브젝트가 꺼져있어 실행이 안됨
    - 게임 오브젝트가 비활성화 상태에서는 코루틴이 작동하지 않음
    - 코루틴 실행전 오브젝트 SetActive를 하여 해결
  - 보스가 산탄총 사용하여 발사하는 기능 추가
  - 몬스터마다 기본 총 추가

- 2023.09.20

  - 탄환을 전부 없애는 스킬 추가
  - 적 - 스폰되었을때 오른쪽으로 총알이 나가는 버그 해결
    - 총의 발사 로직에 1프레임 쉬는 로직 추가
  - 무기 스왑 추가
  - 무기 장탄수 추가

- 2023.09.21

  - 멀티 추가 작업중
    - 포톤으로 작업
    - PUN 2
    - 코루틴은 게임오브젝트가 존재하지 않으면 돌아가지가 않는다.
    - 코루틴을 끄지 않고 씬을 다시 불러와 호출하면 중첩되어 돌아간다 - DontDestroyOnLoad 객체 기준
    - 비동기로 씬 2개를 동시에 불러와 작업하는 로직 추가
  - 멀티 구현중

- 2023.09.26

  - 캐릭터 선택 씬을 타이틀 화면에서 선택하는 방식으로 변경
  - 로딩은 인게임만 하는 것을것으로 변경
  - 각자의 탄창을 인식하고 따로따로 나누기
  - 서로가 서로의 UI에서 자신이 사용할때의 탄창을 표시함
  - 로컬에서의 총의 UI 갱신 추가
  - 멀티에서 자신을 나타내는 식별 삼각형 추가
  - 몬스터 생성 -> 마스터에서 관리 및 게스트 동기화
  - 모든 몬스터에 PhotonView 추가 -> 동기화
  - 맵 확장
  - 엔딩에서 서버를 나가고 다시 게임에 접속할수 있게 변경

- 2023.09.27
  - 버그 : 게스트에서 몬스터가 비활성화 되야하는데 필드에 그대로 나옴
    - 몬스터에서 아무런 필터없이 SetActive(false)를 해주어서 그랬음 -> Check
  - 뷰 아이디의 값이 0이면 RPC를 사용하지 못한다.
    - 뷰 아이디 값이 0이 아니게 만들어야 한다.
  - 멀티 : 적의 경우 커스텀 프로퍼티에 플레이어를 전부 집어 넣고 하나씩 꺼내와서 직접 거리를 비교해서 가장 가까운 플레이어를 추적하게 만든다
  - 싱글 : GameManager.Instanse.PlayerTransform으로 직접 참조하여 타켓팅한다.
