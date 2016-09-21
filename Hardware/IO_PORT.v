`default_nettype none
module IO_PORT(
   input [7:0] addr,
   input RE,
   input WE,
   input [7:0] Din,
   output reg [7:0] Dout,
   output io_read,
   output io_write,
   inout [7:0] IO0,
   inout [7:0] IO1,
   inout [7:0] IO2,
   inout [7:0] IO3,
   inout [7:0] IO4,
   inout [7:0] IO5,
   inout [7:0] IO6,
   inout [7:0] IO7,
   output reg [7:0] io_ena
);

   assign io_read = (addr <= 8'h7) && RE;
   assign io_write = (addr <= 8'h7) && WE;

   wire [7:0] write_ena;

   always @(*)
      if (~WE)
         io_ena = 8'b0;
      else if (addr < 8'h7)
         begin
            io_ena = 8'b0;
            io_ena[addr] = 8'b1;
         end
      else
         io_ena = 8'b0;

   always @(*)
      case (addr)
         8'h0: Dout <= IO0;
         8'h1: Dout <= IO1;
         8'h2: Dout <= IO2;
         8'h3: Dout <= IO3;
         8'h4: Dout <= IO4;
         8'h5: Dout <= IO5;
         8'h6: Dout <= IO6;
         8'h7: Dout <= IO7;
         default: Dout <= 8'bx;
      endcase

   assign IO0 = (addr == 8'h0) && WE ? Din : 8'bz;
   assign IO1 = (addr == 8'h1) && WE ? Din : 8'bz;
   assign IO2 = (addr == 8'h2) && WE ? Din : 8'bz;
   assign IO3 = (addr == 8'h3) && WE ? Din : 8'bz;
   assign IO4 = (addr == 8'h4) && WE ? Din : 8'bz;
   assign IO5 = (addr == 8'h5) && WE ? Din : 8'bz;
   assign IO6 = (addr == 8'h6) && WE ? Din : 8'bz;
   assign IO7 = (addr == 8'h7) && WE ? Din : 8'bz;

endmodule
