@echo off
setlocal

:: 리포지토리 이름 입력받기
set /p repoName="Enter repository name: "

if "%repoName%"=="" (
    echo ❌ 리포지토리 이름을 입력하세요.
    exit /b 1
)

echo 🔧 Setting up Git for Unity project...

:: Git 초기화
git init
git branch -m main

:: Unity .gitignore 다운로드
echo 📄 Downloading Unity .gitignore...
curl -sS https://raw.githubusercontent.com/github/gitignore/main/Unity.gitignore -o .gitignore
git add .gitignore
git commit -m "Add Unity .gitignore"

:: GitHub에 리포지토리 생성
echo 🌍 Creating GitHub repository...
gh repo create "%repoName%" --private --source . --remote origin

echo ✅ Git setup complete!
pause