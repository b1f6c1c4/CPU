`default_nettype none
module Output_divider(
   input Clock,
   input Reset, // fake Reset; auto-start design
   input [15:0] data,
   output reg [3:0] bcd0,
   output reg [3:0] bcd1,
   output reg [3:0] bcd2,
   output reg [3:0] bcd3
   );
   localparam S_C0 = 3'd0;
   localparam S_C1 = 3'd1;
   localparam S_C2 = 3'd2;
   localparam S_C3 = 3'd3;
   localparam S_FN = 3'd4;

   reg [15:0] num;
   wire [15:0] quo, rem;
   assign quo = num / 4'd10;
   assign rem = num % 4'd10;

   reg [2:0] state;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         begin
            num <= 16'b0;
            bcd0 <= 4'b0;
            bcd1 <= 4'b0;
            bcd2 <= 4'b0;
            bcd3 <= 4'b0;
            state <= S_FN;
         end
      else
         case (state)
            S_C0:
               begin
                  num <= quo;
                  bcd0 <= rem;
                  state <= S_C1;
               end
            S_C1:
               begin
                  num <= quo;
                  bcd1 <= rem;
                  state <= S_C2;
               end
            S_C2:
               begin
                  num <= quo;
                  bcd2 <= rem;
                  state <= S_C3;
               end
            S_C3:
               begin
                  num <= quo;
                  bcd3 <= rem;
                  state <= S_FN;
               end
            default:
               begin
                  num <= data;
                  state <= S_C0;
               end
         endcase

endmodule
