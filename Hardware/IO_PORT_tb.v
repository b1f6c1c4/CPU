`default_nettype none
`timescale 10ns/1ps
module IO_PORT_tb;

   reg [7:0] Din;
   reg [7:0] addr;
   reg RE, WE;
   reg [7:0] raw_IO[7:0];
   wire [7:0] Dout;
   wire [7:0] IO[7:0];
   wire io_read, io_write;
   wire [7:0] io_ena;

   IO_PORT mdl(addr, RE, WE, Din, Dout, io_read, io_write, IO[0], IO[1], IO[2], IO[3], IO[4], IO[5], IO[6], IO[7], io_ena);

   integer i;

   genvar j;
   generate
      for (j = 0; j < 8; j = j + 1)
         begin : GEN
            assign IO[j] = ~RE || io_ena[j] ? 8'bz : raw_IO[j];
         end
   endgenerate

   initial
      begin
         addr = 0;
         Din = 8'ha5;
         for (i = 0; i < 8; i = i + 1)
            raw_IO[i] = i * i;

         RE = 1'b0;
         WE = 1'b0;

         #4;

         RE = 1'b0;
         WE = 1'b1;
         addr = 0;
         for (i = 0; i < 10; i = i + 1)
            #4 addr = addr + 8'b1;

         RE = 1'b1;
         WE = 1'b0;
         addr = 0;
         for (i = 0; i < 10; i = i + 1)
            #4 addr = addr + 8'b1;

         #4 $finish;
      end

endmodule
