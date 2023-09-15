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

- 플레이어

  - 총알 발사

- 적

  - 총알 발사

- 보스
  - 총알 발사

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
