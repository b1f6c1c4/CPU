`default_nettype none
module opaque_read_buffer(
   input Clock,
   input Reset,
   input [N-1:0] Din,
   input Din_arrived,
   inout [N-1:0] io,
   input ena
);
   parameter N = 8;

   reg arrived, reading;
   reg [N-1:0] buffer;

   assign io = ena ? 1'bz : reading ? buffer : arrived ? {N{1'b1}} : {N{1'b0}};

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         arrived <= 1'b0;
      else if (ena && reading)
         arrived <= 1'b0;
      else if (Din_arrived)
         arrived <= 1'b1;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         reading <= 1'b0;
      else if (ena && arrived)
         reading <= ~reading;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         buffer <= 1'b0;
      else if (Din_arrived && ~reading)
         buffer <= Din;

endmodule

module opaque_write_buffer(
   input Clock,
   input Reset,
   output [N-1:0] Dout,
   output Dout_send,
   input Dout_ready,
   input Dout_finish,
   inout [N-1:0] io,
   input ena
);
   parameter N = 8;

   reg busy;
   reg [N-1:0] buffer;

   assign Dout_send = ena && ~busy && Dout_ready;
   assign Dout = io;
   assign io = ena ? 1'bz : busy || ~Dout_ready ? {N{1'b0}} : {N{1'b1}};

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         busy <= 1'b0;
      else if (Dout_finish)
         busy <= 1'b0;
      else if (ena && Dout_ready)
         busy <= 1'b1;

endmodule
