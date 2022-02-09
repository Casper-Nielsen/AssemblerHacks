using Jack.Converter;
using Jack.Model;
using Jack.SyntaxAnalyzer;

namespace Jack;

public class JackCompiler
{
    private Tokenizer _tokenizer;
    private Parser _parser;
    private XmlWriter _writer;
    private ConvertToVmModels _converter;

    public JackCompiler()
    {
        _tokenizer = new Tokenizer();
        _parser = new Parser();
        _writer = new XmlWriter();
        _converter = new ConvertToVmModels();
    }

    public void run()
    {
        string jack = @"
class Player{
    field int x;
    field int y;
    field int size;
    field int moveX;
    field int moveY;
    field boolean alive;

    constructor Player new(){
        let x = 15;
        let y = 15;
        let moveX = 1;
        let moveY = 1;
        let size = 15;
        let alive = true;
        do draw( x, y, size);
        return this;
    }

    method void dispose() {
        do Memory.deAlloc(this);
        return;
   }

    method void draw(int drawX, int drawY, int drawSize){
        do Screen.setColor(true);
        do Screen.drawCircle(drawX, drawY, drawSize);
        return;
    }

    method void erase(int drawX, int drawY, int drawSize){
        do Screen.setColor(false);
        do Screen.drawCircle(drawX, drawY, drawSize);
        return;
    }

    method void move(Array array, int length){
        var int i;
        var Player player;
        var int ox;
        var int oy;
        var int os;
        let i = 0;
        if(alive){
            
            
            while(i < length){
                let player = array[i];
                let ox = player.getX();
                let oy = player.getY();
                let os = player.getSize();

                if(colicheck(x,y,ox,oy)){
                    if(coliCheckX(x,y,moveX,size,ox,oy,os)){
                        let moveX = -moveX;
                    }
                    if(coliCheckY(x,y,moveY,size,ox,oy,os)){
                        let moveY = -moveY;
                    }
                }
                let i = i + 1;
            }
            if((x + moveX) > (510 - size)){
                let moveX = -moveX;
            } 
            if((x + moveX) < size){
                let moveX = -moveX;
            } 
            if((y + moveY) > (254 - size)){
                let moveY = -moveY;
            } 
            if((y + moveY) < size){
                let moveY = -moveY;
            } 
            if((moveX + moveY) = 0){
                let alive = true;
            }
            do erase(x,y,size);
            let x = x + moveX;
            let y = y + moveY;
            do draw(x,y,size);
        }
        return;
    }

    method int getX(){
        return x;
    }

    method int getY(){
        return y;
    }

    method int getSize(){
        return size;
    }

    method int getLeftWall(){
        var int leftWall;
        let leftWall = (x - size);
        return leftWall;
    }

    method int getRightWall(){
        var int rightWall;
        let rightWall = (x + 2);
        return rightWall;
    }

    method boolean coliCheckX(int ax, int ay, int amovex, int asize, int bx, int by, int bsize){
        var boolean inY;
        var int aTop;
        var int aBottom;
        var int aRight;
        var int aLeft;
        var int bTop;
        var int bBottom;
        var int bRight;
        var int bLeft;

        let aTop = ay-asize;
        let aBottom = ay+asize;
        let aRight = ax+asize+amovex;
        let aLeft = ax-asize+amovex;

        let bRight = bx+bsize;
        let bLeft = bx-bsize;
        let bTop = by-bsize;
        let bBottom = by+bsize;
        
        if((aTop > bTop) | (aTop = bTop)){
            if((aTop < bBottom) | (aTop = bTop)){
                let inY = true;
            }
        }

        if((aBottom > bTop) | (aBottom = bBottom)){
            if((aBottom < bBottom) | (aBottom = bBottom)){
                let inY = true;
            }
        }

        if(inY){
            if(aRight > bLeft){
                if(aRight < bRight){
                    return true;
                }
            }

            if(aLeft > bLeft){
                if(aLeft < bRight){
                    return true;
                }
            }
        }

        return false;
    }

    method boolean coliCheckY(int ax, int ay, int amovey, int asize, int bx, int by, int bsize){
        var boolean inX;
        var int aTop;
        var int aBottom;
        var int bTop;
        var int bBottom;
        var int aRight;
        var int aLeft;
        var int bRight;
        var int bLeft;
        let aRight = ax+asize;
        let aLeft = ax-asize;
        let aTop = ay-asize+amovey;
        let aBottom = ay+asize+amovey;

        let bRight = bx+bsize;
        let bLeft = bx-bsize;
        let bTop = by-bsize;
        let bBottom = by+bsize;

        if((aRight > bLeft) | (aRight = bRight)){
            if((aRight < bRight) | (aRight = bRight)){
                let inX = true;
            }
        }
        if((aLeft > bLeft) | (aLeft = bLeft)){
            if((aLeft < bRight) | (aLeft = bLeft)){
                let inX = true;
            }
        }

        if(inX){
            if(aTop > bTop){
                if(aTop < bBottom){
                    return true;
                }
            }

            if(aBottom > bTop){
                if(aBottom < bBottom){
                    return true;
                }
            }
        }

        return false;
    }

    method boolean colicheck(int ax,int ay,int bx,int by){
        
        if(ax = bx){
            if(ay = by){
                return false;
            }
        }
        return true;
    }
}
";

        var tokens = _tokenizer.Tokenize(jack);
        var commands = _parser.Parse(tokens);
        var converted = _converter.Convert(commands);
        var str = "";
        foreach (var vmModel in converted)
        {
            str += $"{vmModel.ToString()}\n";
        }
        // _writer.WriteGood(commands);
        
        return;
    }
}