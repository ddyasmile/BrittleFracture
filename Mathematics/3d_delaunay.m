x = gallery('uniformdata',[30 1],0);
y = gallery('uniformdata',[30 1],1);
z = gallery('uniformdata',[30 1],2);
DT = delaunayTriangulation(x,y,z)

tetramesh(DT,'FaceAlpha',0.3);