<p align="center">
    <img src="https://github.com/jhjjgu0115/RimBuff/blob/master/About/Preview.png" alt="JecsTools" />
</p>
<p align="center">
  <a href="https://github.com/jhjjgu0115/RimBuff/releases">
    <img src="https://img.shields.io/badge/release-v0.1-0066cc.svg?style=flat" alt="v0.1" />
  </a>
  <a href="https://github.com/jhjjgu0115/RimBuff/wiki">
    <img src="https://img.shields.io/badge/documentation-Wiki-cc0303.svg?style=flat" alt="Documentation" />
  </a>
</p>

림버프는 림월드에 버프기능을 추가해주는 모드입니다.

ThingWithComp에 CompBuff를 추가하여 개체에 대한 동적 상태변화를 표현할 수 있게 해줍니다.  

CompBuffManager는 다음과 같은 기능들을 제공합니다.  

### 버프 추가
* 새 버프 추가
* 기존의 버프 추가

### 버프 삭제
* 단일 삭제
  * 해당 버프 삭제
  * 해당 이름의 버프 삭제
  * 해당 정의명의 버프 삭제
  * 해당 시전자를 갖는 첫번째 버프 삭제
  * 해당 적용 대상을 갖는 첫번째 버프 삭제
* 다중 삭제
  * 해당 이름을 갖는 모든 버프 삭제
  * 해당 정의명을 갖는 모든 버프 삭제
  * 해당 시전자를 갖는 모든 버프 삭제
  * 해당 적용 대상을 갖는 모든 버프 삭제
### 버프 검색
* 단일 반환
  * 버프로 검색
  * 이름으로 검색
  * 정의명으로 검색
  * 시전자로 검색
  * 적용 대상으로 검색

* 다중 반환
  * 이름으로 검색
  * 정의명으로 검색
  * 시전자로 검색
  * 적용 대상으로 검색
  
### 버프 확인
* 버프로 확인
* 이름으로 확인
* 정의명으로 확인
* 시전자로 확인
* 적용 대상으로 확인
