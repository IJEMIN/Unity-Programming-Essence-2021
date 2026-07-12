# 오류와 오탈자

이 문서는 『레트로의 유니티 게임 프로그래밍 에센스』 개정판의 확인된 오류와 수정 사항을 관리합니다.

- 최종 갱신: 2026-07-13
- 오탈자 제보: [GitHub Issues](https://github.com/IJEMIN/Unity-Programming-Essence-2021/issues) 또는 [한빛미디어 홈페이지](https://www.hanbit.co.kr/store/books/look.php?p_code=B9351446616)

| 페이지 | 위치 및 비고 | 오탈자 | 수정 | 오탈자가 있는 판본/쇄 |
| ---: | --- | --- | --- | --- |
| 009 | 도서 관련 문의 링크 | 도서 관련 문의 디스코드: **discrod.gg/NPXkVq2** | 도서 관련 문의 디스코드: [**retr0.io/discord**](https://retr0.io/discord) | 2쇄 |
| 282 | 과정 03의 1 | 인스펙터 창에서 Bullet(1) 선택 | 하이어라키 창에서 Bullet(1) 선택 |  |
| 302 | 7.5.3 일정 주기로 실행 반복하기 | 따라서 총알을 생성하기 전에 마지막으로 총알을 생성한 시점에서 누적된 시간을 저장하는 변수 **timeSpawnRate**를 체크합니다. | 따라서 총알을 생성하기 전에 마지막으로 총알을 생성한 시점에서 누적된 시간을 저장하는 변수 **timeAfterSpawn**를 체크합니다. | 1판, 2판 |
| 325 | 8.3.1 생존 시간 텍스트 제작<br><br>**비고:** 유니티의 UGUI 텍스트를 생성하는 메뉴가 + \> UI \> Text에서 + \> UI \> Legacy \> Text로 변경됨. | **과정 01 UI 텍스트 만들기**<br>① **씬** 창에서 **2D** 버튼 클릭 → 2D 편집 모드로 전환됨<br>② **하이어라키** 창에서 **+** \> **UI** \> **Text** 클릭 | **과정 01 UI 텍스트 만들기**<br>① **씬** 창에서 **2D** 버튼 클릭 → 2D 편집 모드로 전환됨<br>② **하이어라키** 창에서 **+** \> **UI** \> **Legacy** \> **Text** 클릭 | 1판, 2판 |
| 488 | 페이지 맨 마지막 줄 | **Kenny** Mini Square | **Kenney** Mini Square | 3쇄 |
| 496 | 12.3.4 점수 UI 텍스트 만들기<br><br>**비고:** 유니티의 UGUI 텍스트를 생성하는 메뉴가 + \> UI \> Text에서 + \> UI \> Legacy \> Text로 변경됨. | **과정 01 점수 UI 텍스트 만들기**<br>① 새로운 **Text** 게임 오브젝트 생성 (**+** \> **UI** \> **Text**)<br>② **Text** 게임 오브젝트의 이름을 **Score Text**로 변경<br>③ **Rect Transform** 컴포넌트의 **Width**를 **300**, **Height**를 **80**으로 변경<br>④ **Anchor Preset** 클릭 \> <strong>[Alt+Shift]</strong>를 누른 채로 **bottom-center** 클릭 | **과정 01 점수 UI 텍스트 만들기**<br>① 새로운 **Text** 게임 오브젝트 생성 (**+** \> **UI** \> **Legacy** \> **Text**)<br>② **Text** 게임 오브젝트의 이름을 **Score Text**로 변경<br>③ **Rect Transform** 컴포넌트의 **Width**를 **300**, **Height**를 **80**으로 변경<br>④ **Anchor Preset** 클릭 \> <strong>[Alt+Shift]</strong>를 누른 채로 **bottom-center** 클릭 | 1판, 2판 |
| 583 | 첫째 줄, 임곗값의 영어 | Treshold | Threshold |  |
| 615 | 하단 현재 페이지 정보 | “14장 좀비 서바이버”가 중복 출력됨 | 중복 출력된 페이지 정보 제거 | 1쇄 |
| 686 | p 16.1 다형성 - 4번째 줄 | 16.1 다형성 4번째 줄 **LivignEntity** | **LivingEntity** | 1판, 2판 |
| 737 | 과정 01 아래의 설명 부분 | speed의 값을 zombieData를 통해 입력받은 새로운 속도값인 **newSpeed**의 값으로 변경합니다. | speed의 값을 zombieData를 통해 입력받은 새로운 속도값인 **zombieData.speed**의 값으로 변경합니다. | 1판, 2판 |
| 737 | 과정 01 Setup() 메서드 완성하기 하단 | startingHealth = zombieData.**newHealth**;<br><br>health = zombieData.**newHealth**;<br>damage = zombieData.**newDamage**; | startingHealth = zombieData.**health**;<br><br>health = zombieData.**health**;<br>damage = zombieData.**damage**; | 1판, 2판 |
| 737 | 과정 01 Setup() 메서드 완성하기의 코드 부분 | // 체력 설정<br>startingHealth = zombieData.health;<br>health = **zombieData.damage;**<br>// 공격력 설정<br>damage = zombieData.damage<br>// 내비메시 에이전트의 이동 속도 설정<br>**pathMeshAgent**.speed = zombieData.speed; | // 체력 설정<br>startingHealth = zombieData.health;<br>health = **zombieData.health;**<br>// 공격력 설정<br>damage = zombieData.damage<br>// 내비메시 에이전트의 이동 속도 설정<br>**navMeshAgent**.speed = zombieData.speed; | 1판, 2판 |
| 744 | 맨 마지막 줄 | nevMeshAgent.isStopped = true; | navMeshAgent.isStopped = true; | 1판, 2판 |
| 905 | 과정 01의 3, Resources 폴더로 프리팹 옮기기 그림 위 | 선택한 프리팹을 **Resource** 폴더로 옮김 | 선택한 프리팹을 **Resources** 폴더로 옮김 | PDF 2쇄 |
