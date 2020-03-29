import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;

public class Graph {

    private int vertNum;
    private int edgeNum;
    private Edge[] edges;
    private String[] vertices;

    public Graph(){

    }

    public Graph(int vertNum, int edgeNum){
        this.vertNum = vertNum;
        this.edgeNum = edgeNum;
    }

    //Method to initialize the Graph
    //It receives a path to a File that contains the edges of the Graph
    //First line contains verNum and edgeNum
    //Following lines represent the edges in the format:
    // src dest weight
    public void initializeByFile(String path){
        try {
            BufferedReader br = new BufferedReader(new FileReader(path));
            String line = br.readLine();
            String[] entryValues = line.split(" ");
            this.vertNum = Integer.parseInt(entryValues[0]);
            this.edgeNum = Integer.parseInt(entryValues[1]);
            this.vertices = new String[vertNum];
            this.edges = new Edge[edgeNum];
            int iteration = 0;

            //Sets the name of the vertices
            for(int i = 0; i < this.vertNum; i++){
                line = br.readLine();
                String[] aux = line.split(" ");
                vertices[Integer.parseInt(aux[0])] = aux[1];
            }

            while((line = br.readLine()) != null){
                String[] edgeValues = line.split(" ");
                edges[iteration] = new Edge();
                edges[iteration].setSrc(Integer.parseInt(edgeValues[0]));
                edges[iteration].setDest(Integer.parseInt(edgeValues[1]));
                edges[iteration].setWeight(Integer.parseInt(edgeValues[2]));
                iteration++;
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public String[] getVertNames(){
        return this.vertices;
    }

    public Edge[] getEdges() {
        return edges;
    }

    public int getVertNum() {
        return vertNum;
    }

    public int getEdgeNum() {
        return edgeNum;
    }

}
