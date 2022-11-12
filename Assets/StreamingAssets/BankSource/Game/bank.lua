
function Init()
    require("Libs/tween.lua")
    require("Libs/hooks.lua")
	
	game.Log("Hello from Game Bank Lua!")
	
	hooks.Fire("PostGameInit")
end

function CurTime()
	return game.CurTime()
end

function DeltaTime()
	return game.DeltaTime();
end

function Update()
	hooks.Fire("OnUpdate")
end