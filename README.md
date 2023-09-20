# Multi_Shooting_Game

# Resource Site

- https://www.kenney.nl/assets/category:2D
- https://resourcebank.or.kr/?menucode=30100&tmenu=graphics&mode=&catemenug=111&orderby=&s_type=&s_key=%ED%94%BD%EC%85%80
- https://opengameart.org/
- https://assetstore.unity.com/?category=2d%2Fcharacters&free=true&orderBy=1&page=3

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
