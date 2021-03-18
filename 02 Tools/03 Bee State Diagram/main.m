%main.m
missing_value = nan; 
data = load('data.txt'); 
timeser = data(:,1); 

minDate = min(timeser ); 
maxDate = max(timeser); 

entiretime = [0:1:1000000]';
tm_missed = setdiff(entiretime,timeser); 
line_missed = data(1,:); 
line_missed(2) = missing_value; 
rows_missed = repmat(line_missed, length(tm_missed), 1); 
rows_missed(:,1)=tm_missed; 

data = [data; rows_missed]; 
data = sortrows(data,1); 
F = fillmissing(data,'previous');
stem(F(:,1),F(:,2));
fid=fopen('data_out.txt','w');
fprintf(fid,'%d\t%d\r\n',data');
fclose(fid);
