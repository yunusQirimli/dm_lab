public class Edge {

    private int src;
    private String srcName;
    private int dest;
    private int desrName;
    private int weight;

    public String getSrcName() {
        return srcName;
    }

    public void setSrcName(String srcName) {
        this.srcName = srcName;
    }

    public int getDesrName() {
        return desrName;
    }

    public void setDesrName(int desrName) {
        this.desrName = desrName;
    }

    public int getSrc() {
        return src;
    }

    public void setSrc(int src) {
        this.src = src;
    }

    public int getDest() {
        return dest;
    }

    public void setDest(int dest) {
        this.dest = dest;
    }

    public int getWeight() {
        return weight;
    }

    public void setWeight(int weight) {
        this.weight = weight;
    }
}
