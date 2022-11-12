assert(hooks)
dlc0 = {}

dlc0.Init = function()

end

dlc0.Update = function()

end

hooks.Add("PostGameInit", dlc0.Init)
hooks.Add("OnUpdate", dlc0.Update)
