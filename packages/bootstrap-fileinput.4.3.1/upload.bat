@echo off

call build

REM Upload the new package
for %%f in (bootstrap-fileinput.*) do (
NuGet Push %%f
rmdir /s /q content
del %%f
)
