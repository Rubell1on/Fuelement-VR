main(false);

function main(reset)
    try
        clc
        disp("Starting up");

        if exist('fis') == false || reset == true
            clear all;
            fis = loadFis("../model_trained_50_epochs_named");
        end

        t = createConnection('localhost', 3000);
        while true
            while t.BytesAvailable == 0
               %pause(0.01);
            end
            data = readData(fread(t, t.BytesAvailable).', 'UTF-8')
            disp(data);
            res = evalfis(fis, [data.rpm, data.gear, data.speed, data.deltaRpm]);
            resStruct = struct("response", res);
            resString = jsonencode(resStruct);
            disp(res);
            flushinput(t);
            fwrite(t, resString);
        end
        fclose(t);
        clear t;
    catch ME
        disp(ME.identifier)
    end
end

function fis = loadFis(path)
    disp('Loading FIS');
    fis = readfis(path);
    disp('FIS loaded!');
end

function t = createConnection(address, port)
    disp('Openning connection');
    t = tcpip(address, port, 'NetworkRole', 'server', 'Terminator', 'CR');
    disp('Waiting for connection');
    fopen(t);
    disp('Connection established');
end

function data = readData(stream, encoding)
    str = native2unicode(stream, encoding);
    data = matlab.internal.webservices.fromJSON(str)
    %data = jsondecode(str);
end
