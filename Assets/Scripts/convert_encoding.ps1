# Unity C# 스크립트 파일 인코딩을 UTF-8로 변환하는 스크립트
# 사용법: .\convert_encoding.ps1

Write-Host "Unity C# 스크립트 파일 인코딩 변환 시작..." -ForegroundColor Green

# 현재 디렉토리에서 모든 .cs 파일 찾기
$csFiles = Get-ChildItem -Path . -Filter "*.cs" -Recurse

$convertedCount = 0

foreach ($file in $csFiles) {
    try {
        # 파일 내용 읽기 (기본 인코딩으로)
        $content = Get-Content -Path $file.FullName -Raw -Encoding Default
        
        # UTF-8로 다시 저장
        $content | Out-File -FilePath $file.FullName -Encoding UTF8
        
        Write-Host "변환 완료: $($file.Name)" -ForegroundColor Yellow
        $convertedCount++
    }
    catch {
        Write-Host "오류 발생: $($file.Name) - $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "변환 완료! 총 $convertedCount 개 파일이 변환되었습니다." -ForegroundColor Green
Write-Host "이제 Cursor에서 한글이 제대로 표시될 것입니다." -ForegroundColor Cyan 