`default_nettype none
module UART_WriteD(
   input Clock,
   input Reset,
   output ready,
   input send,
   output reg finish,
   input [7:0] data,
   output TX
   );
`ifdef SIMULATION
   parameter div = 24;
`else
   parameter div = 217; // 115200
`endif

   localparam S_IDLE = 1'h0;
   localparam S_SEND = 1'h1;

   reg [9:0] shift_reg;
   reg [31:0] cnt_freq;
   reg [3:0] cnt_bit;

   reg state;
   reg send_tr, pre_send;

   assign ready = Reset & (state == S_IDLE);
   assign TX = (state != S_SEND) | shift_reg[0];

   always @(negedge Clock, negedge Reset)
      if (~Reset)
         begin
            pre_send <= 1'b0;
            send_tr <= 1'b0;
         end
      else
         begin
            send_tr <= 1'b0;
            if(send && ~pre_send)
               send_tr <= 1'b1;
            pre_send <= send;
         end

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         state <= S_IDLE;
      else if (state == S_IDLE && send_tr)
         state <= S_SEND;
      else if (state == S_SEND && ~|cnt_bit && ~|cnt_freq)
         state <= S_IDLE;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         shift_reg <= 10'b0;
      else if (state == S_IDLE && send_tr)
         shift_reg <= {1'b1,data,1'b0};
      else if (state == S_SEND && ~|cnt_freq)
         shift_reg <= shift_reg >> 1;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         cnt_bit <= 4'd9;
      else if (state == S_IDLE)
         cnt_bit <= 4'd9;
      else if (state == S_SEND && ~|cnt_freq)
         cnt_bit <= cnt_bit - 4'd1;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         cnt_freq <= div - 1;
      else if (state == S_SEND)
         cnt_freq <= ~|cnt_freq ? div - 1 : cnt_freq - 1;
      else
         cnt_freq <= div - 1;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         finish <= 1'b0;
      else if (state == S_SEND && ~|cnt_bit && ~|cnt_freq)
         finish <= 1'b1;
      else
         finish <= 1'b0;

endmodule

module UART_ReadD(
   input Clock,
   input Reset,
   output arrived,
   output reg [7:0] data,
   input RX
   );
`ifdef SIMULATION
   parameter div = 2;
`else
   parameter div = 18; // 12x 115200
`endif

   localparam S_IDLE = 4'h0;
   localparam S_BITS = 4'h1;
   localparam S_BIT0 = 4'h2;
   localparam S_BIT1 = 4'h3;
   localparam S_BIT2 = 4'h4;
   localparam S_BIT3 = 4'h5;
   localparam S_BIT4 = 4'h6;
   localparam S_BIT5 = 4'h7;
   localparam S_BIT6 = 4'h8;
   localparam S_BIT7 = 4'h9;
   localparam S_BITX = 4'ha;

   reg [3:0] state;
   reg [31:0] cnt_freq;
   reg [3:0] cnt_wait;
   reg [7:0] shift_reg;

   assign arrived = (state == S_BITX) && waitx;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         data <= 8'b0;
      else if (state == S_BITX && ~|cnt_freq)
         data <= shift_reg;

   wire waitx = ~(|cnt_freq || |cnt_wait);

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         state <= S_IDLE;
      else if (state == S_IDLE)
         begin
            if (~RX)
               state <= S_BITS;
         end
      else if (waitx)
         case (state)
            S_BITS: state <= S_BIT0;
            S_BIT0: state <= S_BIT1;
            S_BIT1: state <= S_BIT2;
            S_BIT2: state <= S_BIT3;
            S_BIT3: state <= S_BIT4;
            S_BIT4: state <= S_BIT5;
            S_BIT5: state <= S_BIT6;
            S_BIT6: state <= S_BIT7;
            S_BIT7: state <= S_BITX;
            S_BITX: state <= S_IDLE;
         endcase

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         shift_reg <= 8'b0;
      else if (waitx)
         case (state)
            S_BITS, S_BIT0, S_BIT1, S_BIT2, S_BIT3, S_BIT4, S_BIT5, S_BIT6, S_BIT7:
               shift_reg <= {RX,shift_reg[7:1]};
         endcase

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         cnt_wait <= 4'd0;
      else
         case (state)
            S_IDLE:
               cnt_wait <= 4'd4;
            S_BITS, S_BIT0, S_BIT1, S_BIT2, S_BIT3, S_BIT4, S_BIT5, S_BIT6, S_BIT7:
               if (waitx)
                  cnt_wait <= 4'd11;
               else if (~|cnt_freq)
                  cnt_wait <= cnt_wait - 4'd1;
            S_BITX:
               if (~waitx && ~|cnt_freq)
                  cnt_wait <= cnt_wait - 4'd1;
         endcase

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         cnt_freq <= div - 1;
      else if (state == S_IDLE)
         cnt_freq <= div - 1;
      else
         cnt_freq <= ~|cnt_freq ? div - 1 : cnt_freq - 1;

endmodule
