SET DEFINE OFF;

-- ===== CATEGORY =====
INSERT INTO Category (name, description) VALUES ('GÀ - GÀ QUAY', 'Các loại gà rán, gà quay');
INSERT INTO Category (name, description) VALUES ('BURGER - CƠM - MÌ Ý', 'Các loại bánh burger, Cơm gà, mì ý');
INSERT INTO Category (name, description) VALUES ('THỨC ĂN NHẸ', 'Các loại salad, khoai tây');
INSERT INTO Category (name, description) VALUES ('THỨC UỐNG - TRÁNG MIỆNG', 'Các loại nước ngọt, bánh ngọt');
-- ===== PRODUCT =====    
    -- Category 1 : GÀ - GÀ QUAY
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('1 Miếng gà rán', '1 Miếng Gà Giòn Cay/Gà Truyền Thống/Gà Giòn Không Cay + 1 Gói tương (cà/ ớt)', 35000, 'https://static.kfcvietnam.com.vn/images/items/lg/1-GA-XOT.jpg?v=gk7XPg', 2, 1);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('2 Miếng gà rán', '2 Miếng Gà Giòn Cay/Gà Truyền Thống/Gà Giòn Không Cay + 2 Gói tương (cà/ ớt)', 70000, 'https://static.kfcvietnam.com.vn/images/items/lg/2-GA-XOT.jpg?v=gk7XPg', 10, 1);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('3 Miếng gà rán', '3 Miếng Gà Giòn Cay/Gà Truyền Thống/Gà Giòn Không Cay + 3 Gói tương (cà/ ớt)', 104000, 'https://static.kfcvietnam.com.vn/images/items/lg/3-GA-XOT.jpg?v=gk7XPg', 10, 1);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('6 Miếng gà rán', '6 Miếng Gà Giòn Cay/Gà Truyền Thống/Gà Giòn Không Cay + 6 Gói tương (cà/ ớt)', 205000, 'https://static.kfcvietnam.com.vn/images/items/lg/6-GA-XOT.jpg?v=gk7XPg', 10, 1);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('1 Miếng Phi-lê Gà Quay', '1 Miếng Phi-lê Gà Quay Flava/Phi-lê Gà Quay Tiêu', 42000, 'https://static.kfcvietnam.com.vn/images/items/lg/PHILE-XOT.jpg?v=gk7XPg', 10, 1);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Gà Viên (Vừa)', 'Gà Viên (Vừa) + 1 Gói tương (cà/ ớt)', 38000, 'https://static.kfcvietnam.com.vn/images/items/lg/POPCORN-XOT.jpg?v=gk7XPg', 10, 1);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Gà Viên (Lớn)', 'Gà Viên (Lớn) + 2 Gói tương (cà/ ớt)', 64000, 'https://static.kfcvietnam.com.vn/images/items/lg/POP-L.jpg?v=gk7XPg', 10, 1);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('3 Gà Miếng Nuggets', '3 Gà Miếng Nuggets + 1 Gói tương (cà/ ớt)', 27000, 'https://static.kfcvietnam.com.vn/images/items/lg/3_Nuggests.jpg?v=gk7XPg', 10, 1);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('5 Gà Miếng Nuggets', '5 Gà Miếng Nuggets + 2 Gói tương (cà/ ớt)', 40000, 'https://static.kfcvietnam.com.vn/images/items/lg/5_Nuggests.jpg?v=gk7XPg', 10, 1);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('10 Gà Miếng Nuggets', '10 Gà Miếng Nuggets + 4 Gói tương (cà/ ớt)', 75000, 'https://static.kfcvietnam.com.vn/images/items/lg/10_Nuggests.jpg?v=gk7XPg', 10, 1);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('3 Miếng Gà Rán Tender', '3 Miếng Gà Rán Tender + 1 Gói tương (cà/ ớt)', 41000, 'https://static.kfcvietnam.com.vn/images/items/lg/TENDERS-3.jpg?v=gk7XPg', 10, 1);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('5 Miếng Gà Rán Tender', '5 Miếng Gà Rán Tender + 2 Gói tương (cà/ ớt)', 66000, 'https://static.kfcvietnam.com.vn/images/items/lg/TENDERS-5.jpg?v=gk7XPg', 10, 1);

    -- Category 2 : BURGER - CƠM - MÌ Ý
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Burger Zinger', '1 Burger Zinger +1 Gói tương (cà/ ớt)', 54000, 'https://static.kfcvietnam.com.vn/images/items/lg/Burger-Zinger.jpg?v=gk7XPg', 10, 2);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Burger Tôm', '1 Burger Tôm + 1 Gói tương (cà/ ớt)', 45000, 'https://static.kfcvietnam.com.vn/images/items/lg/Burger-Shrimp.jpg?v=gk7XPg', 10, 2);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Burger Gà Quay Flava', '1 Burger Gà Quay Flava + 1 Gói tương (cà/ ớt)', 54000, 'https://static.kfcvietnam.com.vn/images/items/lg/Burger-Flava.jpg?v=gk7XPg', 10, 2);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Cơm Gà Teriyaki', '1 Cơm Gà Teriyaki + 1 Gói tương (cà/ ớt)', 45000, 'https://static.kfcvietnam.com.vn/images/items/lg/Rice-Teriyaki.jpg?v=gk7XPg', 10, 2);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Cơm Gà Rán', '1 Cơm Gà Rán + 1 Gói tương (cà/ ớt)', 49000, 'https://static.kfcvietnam.com.vn/images/items/lg/Rice-F.Chicken.jpg?v=gk7XPg', 10, 2);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Cơm Phi-lê Gà Quay', '1 Cơm Phi-lê Gà Quay + 1 Gói tương (cà/ ớt)', 49000, 'https://static.kfcvietnam.com.vn/images/items/lg/Rice-Flava.jpg?v=gk7XPg', 10, 2);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Cơm', '1 Cơm', 12000, 'https://static.kfcvietnam.com.vn/images/items/lg/Rice.jpg?v=gk7XPg', 10, 2);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Mì Ý Gà Viên', '1 Mì Ý Gà Viên + 1 Gói tương (cà/ ớt) ', 40000, 'https://static.kfcvietnam.com.vn/images/items/lg/MI-Y-GA-VIEN.jpg?v=gk7XPg', 10, 2);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Mì Ý Gà Rán', '1 Mì Ý Gà Rán + + 1 Gói tương (cà/ ớt)', 64000, 'https://static.kfcvietnam.com.vn/images/items/lg/MI-Y-GA-RAN.jpg?v=gk7XPg', 10, 2);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Cơm Gà Viên Nanban', '1 Cơm Gà Viên Nanban + 1 Gói tương (cà/ ớt)', 12000, 'https://static.kfcvietnam.com.vn/images/items/lg/NANBAN.jpg?v=gk7XPg', 10, 2);

    -- Category 3 : THỨC ĂN NHẸ
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Salad Hạt', '1 Salad Hạt ', 39000, 'https://static.kfcvietnam.com.vn/images/items/lg/SALAD-HAT.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Salad Pop', '1 Salad Hạt Gà Viên Popcorn', 45000, 'https://static.kfcvietnam.com.vn/images/items/lg/SALAD-HAT-GA-VIEN.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('3 Cá Thanh', '3 Cá Thanh + 1 Gói tương (cà/ ớt)', 40000, 'https://static.kfcvietnam.com.vn/images/items/lg/3-FISH-STICK.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('4 Phô Mai Viên', '4 Phô Mai Viên + 1 Gói tương (cà/ ớt)', 36000, 'https://static.kfcvietnam.com.vn/images/items/lg/4-Chewy-Cheese.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('6 Phô Mai Viên', '6 Phô Mai Viên + 1 Gói tương (cà/ ớt)', 49000, 'https://static.kfcvietnam.com.vn/images/items/lg/6-Chewy-Cheese.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Khoai Tây Chiên (Vừa)', 'Khoai Tây Chiên (Vừa) + 1 Gói tương (cà/ ớt)', 19000, 'https://static.kfcvietnam.com.vn/images/items/lg/FF-R.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Khoai Tây Chiên (Lớn)', 'Khoai Tây Chiên (Lớn) + 1 Gói tương (cà/ ớt)', 29000, 'https://static.kfcvietnam.com.vn/images/items/lg/FF-L.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Khoai Tây Chiên (Đại)', 'Khoai Tây Chiên (Đại) + 2 Gói tương (cà/ ớt)', 39000, 'https://static.kfcvietnam.com.vn/images/items/lg/FF-J.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Khoai Tây Múi Cau (Vừa)', '01 Khoai Tây Múi Cau (vừa) + 1 Gói tương (cà/ ớt)', 23000, 'https://static.kfcvietnam.com.vn/images/items/lg/khoai-mui-cau-R.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Khoai Tây Múi Cau (Lớn)', '01 Khoai Tây Múi Cau (lớn) + 1 Gói tương (cà/ ớt)', 43000, 'https://static.kfcvietnam.com.vn/images/items/lg/khoai-mui-cau-L.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Khoai Tây Nghiền (Vừa)', 'Khoai Tây Nghiền (Vừa)', 12000, 'https://static.kfcvietnam.com.vn/images/items/lg/MP-(R)-new.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Khoai Tây Nghiền (Lớn)', 'Khoai Tây Nghiền (Lớn)', 22000, 'https://static.kfcvietnam.com.vn/images/items/lg/MP-(L)-new.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Khoai Tây Nghiền (Đại)', 'Khoai Tây Nghiền (Đại)', 31000, 'https://static.kfcvietnam.com.vn/images/items/lg/MP-(J)-new.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Bắp Cải Trộn (Vừa)', 'Bắp Cải Trộn (Vừa)', 12000, 'https://static.kfcvietnam.com.vn/images/items/lg/CL-(R)-new.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Bắp Cải Trộn (Lớn)', 'Bắp Cải Trộn (Lớn)', 22000, 'https://static.kfcvietnam.com.vn/images/items/lg/CL-(L)-new.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Bắp Cải Trộn (Đại)', 'Bắp Cải Trộn (Đại)', 31000, 'https://static.kfcvietnam.com.vn/images/items/lg/CL-(J)-new.jpg?v=gk7XPg', 10, 3);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('Súp Rong Biển', 'Súp Rong Biển', 19000, 'https://static.kfcvietnam.com.vn/images/items/lg/Soup-Rong-Bien.jpg?v=gk7XPg', 10, 3);

    -- Category 4 : THỨC UỐNG - TRÁNG MIỆNG
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES 
('1 Bánh Trứng', '1 Bánh Trứng', 18000, 'https://static.kfcvietnam.com.vn/images/items/lg/EGGTART-1.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('4 Bánh Trứng', '4 Bánh Trứng', 64000, 'https://static.kfcvietnam.com.vn/images/items/lg/EGGTART-4.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('2 Viên Khoai Môn Kim Sa', '2 Viên Khoai Môn Kim Sa', 26000, 'https://static.kfcvietnam.com.vn/images/items/lg/2-taro.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('3 Viên Khoai Môn Kim Sa', '3 Viên Khoai Môn Kim Sa', 34000, 'https://static.kfcvietnam.com.vn/images/items/lg/3-taro.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('5 Viên Khoai Môn Kim Sa', '5 Viên Khoai Môn Kim Sa', 54000, 'https://static.kfcvietnam.com.vn/images/items/lg/5-taro.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Pepsi Lon', 'Pepsi Lon', 19000, 'https://static.kfcvietnam.com.vn/images/items/lg/PEPSI_CAN.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('7Up Lon', '7Up Lon', 19000, 'https://static.kfcvietnam.com.vn/images/items/lg/7UP_CAN.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Aquafina 500ml', 'Aquafina 500ml', 15000, 'https://static.kfcvietnam.com.vn/images/items/lg/AQUAFINA.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Pepsi Không Calo Lon', 'Pepsi Không Calo Lon', 19000, 'https://static.kfcvietnam.com.vn/images/items/lg/pepsi-zero.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Lon Sting', 'Lon Sting', 19000, 'https://static.kfcvietnam.com.vn/images/items/lg/Sting.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Pepsi (Tiêu Chuẩn)', '1 Ly Pepsi (Tiêu Chuẩn)', 12000, 'https://static.kfcvietnam.com.vn/images/items/lg/PEPSI-STD.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Pepsi (Vừa)', '1 Ly Pepsi (Vừa)', 15000, 'https://static.kfcvietnam.com.vn/images/items/lg/PEPSI-M.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Pepsi (Đại)', '1 Ly Pepsi (Đại)', 19000, 'https://static.kfcvietnam.com.vn/images/items/lg/PEPSI-J.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('7Up (Tiêu Chuẩn)', '1 Ly 7Up (Tiêu Chuẩn)', 12000, 'https://static.kfcvietnam.com.vn/images/items/lg/7UP-STD.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('7Up (Vừa)', '1 Ly 7Up (Vừa)', 15000, 'https://static.kfcvietnam.com.vn/images/items/lg/7UP-R.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('7Up (Đại)', '1 Ly 7Up (Đại)', 19000, 'https://static.kfcvietnam.com.vn/images/items/lg/7UP-L.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Lipton (Tiêu Chuẩn)', '1 Ly Lipton (Tiêu Chuẩn)', 12000, 'https://static.kfcvietnam.com.vn/images/items/lg/LIPTON-STD.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Lipton (Vừa)', '1 Ly Lipton (Vừa)', 15000, 'https://static.kfcvietnam.com.vn/images/items/lg/LIPTON-M.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Lipton (Đại)', '1 Ly Lipton (Đại)', 19000, 'https://static.kfcvietnam.com.vn/images/items/lg/LIPTON-J.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Pepsi Không Đường (Tiêu Chuẩn)', '1 Ly Pepsi Không Đường (Tiêu Chuẩn)', 12000, 'https://static.kfcvietnam.com.vn/images/items/lg/PEPSI-ZERO-STD.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Pepsi Không Đường (Vừa)', '1 Ly Pepsi Không Đường (Vừa)', 15000, 'https://static.kfcvietnam.com.vn/images/items/lg/PEPSI-ZERO-M.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Pepsi Không Đường (Đại)', '1 Ly Pepsi Không Đường (Đại)', 19000, 'https://static.kfcvietnam.com.vn/images/items/lg/PEPSI-ZERO-J.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Sô-cô-la Sữa Đá', '1 Ly Sô-cô-la Sữa Đá', 20000, 'https://static.kfcvietnam.com.vn/images/items/lg/CHOCO-MILK-STD.jpg?v=gk7XPg', 10, 4);
INSERT INTO Product (name, description, price, image_url, inventory, category_id) VALUES
('Sô-cô-la Sữa Nóng', '1 Ly Sô-cô-la Sữa Nóng', 20000, 'https://static.kfcvietnam.com.vn/images/items/lg/ChoCo_Hot.jpg?v=gk7XPg', 10, 4);

-- ===== COMBO =====
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo 1 Miếng Gà', '1 Miếng Gà Rán + 1 Khoai Tây Chiên (Vừa)/ 1 Khoai Tây Nghiền (Vừa) & 1 Bắp Cải Trộn (Vừa) + 1 Pepsi (Tiêu chuẩn) + 2 Gói tương (cà/ ớt)', 58000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-CHICKEN-1.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo 2 Miếng Gà', '2 Miếng Gà Rán + 1 Khoai Tây Chiên (Vừa)/1 Khoai Tây Nghiền (Vừa) & 1 Bắp Cải Trộn (Vừa) + 1 Ly Pepsi (Tiêu chuẩn) + 3 Gói tương (cà/ ớt)', 95000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-CHICKEN-2.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Gà Rán Tender', '3 Miếng Gà Rán Tenders + 1 Xà lách Hạt + 1 Pepsi Không Đường (Tiêu chuẩn) + 1 Gói tương (cà/ ớt)', 85000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-TENDER.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Phi-lê Gà Quay', '1 Miếng Phi-lê Gà Quay + 1 Bắp Cải Trộn (Lớn) + 1 Pepsi Không Đường (Tiêu chuẩn) + 1 Gói tương (cà/ ớt)', 69000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-ROASTED.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Mì Ý Gà Viên', '1 Mì Ý Gà Viên + 1 Pepsi (Tiêu chuẩn) + 1 Gói tương (cà/ ớt)', 45000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-PASTA-POP.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Mì Ý Gà Rán', '1 Mì Ý Gà Rán + 1 Ly Pepsi (Tiêu chuẩn) + 1 Gói tương (cà/ ớt)', 72000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-PASTA-COB.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Mì Ý và Gà Tender', '1 Mì Ý Gà Viên + 2 Miếng Gà Rán Tenders + 1 Khoai Tây Chiên (Vừa) + 1 Pepsi (Tiêu chuẩn) + 3 Gói tương (cà/ ớt)', 95000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-PASTA-TENDERS.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Mì Ý và Salad Gà', '1 Mì Ý Gà Viên + 1 Xà Lách Gà Viên + 1 Pepsi (Tiêu chuẩn) + 1 Gói tương (cà/ ớt)', 85000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-PASTA-SALAD.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Cơm Gà Rán', '1 Cơm Gà Rán + 1 Ly Pepsi (Tiêu chuẩn) + 1 Gói tương (cà/ ớt)', 59000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-RICE-COB.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Cơm Gà Quay', '1 Cơm Gà Flava + 1 Ly Pepsi (Tiêu chuẩn) + 1 Gói tương (cà/ ớt)', 59000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-RICE-ROASTED.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Cơm Gà Nanban', '1 Cơm Gà Nanban + 1 Ly Pepsi (Tiêu chuẩn) + 1 Gói tương (cà/ ớt)', 49000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-RICE-NANBAN.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Cơm Gà Nanban và Súp Rong Biển', '1 Cơm Gà Nanban + 1 Súp Rong Biển + 1 Ly Pepsi (Tiêu chuẩn) + 1 Gói tương (cà/ ớt)', 68000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-RICE-NANBAN-SOUP.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Burger Tôm', '1 Burger Tôm + 1 Ly Pepsi (Tiêu chuẩn) + 1 Gói tương (cà/ ớt)', 50000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-SHRIMP.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Burger Gà Zinger và Khoai', '1 Burger Zinger + 1 Khoai Tây Chiên (Vừa) + 1 Ly Pepsi (Tiêu chuẩn) + 2 Gói tương (cà/ ớt)', 77000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-B.ZINGER-FF.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Burger Phi-lê Gà Quay và Khoai', '1 Burger Gà Quay + 1 Khoai Tây Chiên (Vừa) + 1 Ly Pepsi (Tiêu chuẩn) + 2 Gói tương (cà/ ớt)', 77000, 'https://static.kfcvietnam.com.vn/images/items/lg/DB-ROASTED-FF.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Burger Tôm & Gà Rán', '1 Burger Tôm + 1 Miếng Gà Rán + 1 Ly Pepsi (Tiêu chuẩn) + 2 Gói tương (cà/ ớt)', 870000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-B.SHRIMP-COB.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Burger Phi-lê Gà Quay và Gà Rán', '1 Burger Phi-lê Gà Quay + 1 Miếng Gà Rán + 1 Ly Pepsi (Tiêu chuẩn) + 2 Gói tương (cà/ ớt)', 95000, 'https://static.kfcvietnam.com.vn/images/items/lg/DB-ROASTED-CBO.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Burger Gà Zinger và Gà Rán', '1 Burger Zinger + 1 Miếng Gà Rán + 1 Ly Pepsi (Tiêu chuẩn) + 2 Gói tương (cà/ ớt)', 95000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-B.ZINGER-COB.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Burger và Gà Rán', '1 Burger Gà Quay/Zinger + 1 Khoai Tây Chiên (Vừa) + 1 Miếng Gà Rán + 1 Ly Pepsi (Tiêu chuẩn) + 3 Gói tương (cà/ ớt)', 112000, 'https://static.kfcvietnam.com.vn/images/items/lg/D-BURGER-COB-FF.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Nhóm 2 Hoàn Hảo', '2 Miếng Gà Rán + 1 Burger Zinger + 2 Ly Pepsi (Tiêu chuẩn)+ 3 Gói tương (cà/ ớt)', 135000, 'https://static.kfcvietnam.com.vn/images/items/lg/DBUCKET1.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Nhóm 2 Tròn Vị ', '3 Miếng Gà Rán + 1 Mì Ý Gà Viên + 2 Ly Pepsi (Tiêu chuẩn) + 4 Gói tương (cà/ ớt)', 160000, 'https://static.kfcvietnam.com.vn/images/items/lg/DBUCKET5.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Nhóm 2 No Nê', '4 Miếng Gà Rán + 1 Khoai Múi Cau (Vừa) + 2 Ly Pepsi (Tiêu chuẩn) + 5 Gói tương (cà/ ớt)', 179000, 'https://static.kfcvietnam.com.vn/images/items/lg/DBUCKET4.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Nhóm 3 Đủ Đầy', '3 Miếng Gà Rán + 1 Mì Ý Gà Viên + 1 Burger Tôm + 1 Khoai Tây Chiên (Vừa) + 3 Ly Pepsi (Tiêu chuẩn) + 6 Gói tương (cà/ ớt)', 219000, 'https://static.kfcvietnam.com.vn/images/items/lg/DBUCKET2.jpg?v=gk7XPg');
INSERT INTO Combo (name, description, price, image_url)
VALUES ('Combo Nhóm 5 Hội Tụ', '6 Miếng Gà Rán + 1 Mì Ý Gà Viên + 1 Khoai Múi Cau (Vừa) + 5 Ly Pepsi (Tiêu chuẩn) + 8 Gói tương (cà/ ớt)', 309000, 'https://static.kfcvietnam.com.vn/images/items/lg/DBUCKET3.jpg?v=gk7XPg');

-- ===== COMBO ITEM =====

-- Combo 1: Combo 1 Miếng Gà (combo_id = 1)
-- 1 Miếng Gà Rán + 1 Khoai Tây Chiên (Vừa) + 1 Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (1, 1, 1);  -- 1 Miếng gà rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (1, 26, 1); -- Khoai Tây Chiên (Vừa)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (1, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 2: Combo 2 Miếng Gà (combo_id = 2)
-- 2 Miếng Gà Rán + 1 Khoai Tây Chiên (Vừa) + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (2, 2, 1);  -- 2 Miếng gà rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (2, 26, 1); -- Khoai Tây Chiên (Vừa)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (2, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 3: Combo Gà Rán Tender (combo_id = 3)
-- 3 Miếng Gà Rán Tenders + 1 Xà lách Hạt + 1 Pepsi Không Đường (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (3, 11, 1); -- 3 Miếng Gà Rán Tender
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (3, 21, 1); -- Salad Hạt
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (3, 50, 1); -- Pepsi Không Đường (Tiêu Chuẩn)

-- Combo 4: Combo Phi-lê Gà Quay (combo_id = 4)
-- 1 Miếng Phi-lê Gà Quay + 1 Bắp Cải Trộn (Lớn) + 1 Pepsi Không Đường (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (4, 5, 1);  -- 1 Miếng Phi-lê Gà Quay
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (4, 33, 1); -- Bắp Cải Trộn (Lớn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (4, 50, 1); -- Pepsi Không Đường (Tiêu Chuẩn)

-- Combo 5: Combo Mì Ý Gà Viên (combo_id = 5)
-- 1 Mì Ý Gà Viên + 1 Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (5, 18, 1); -- Mì Ý Gà Viên
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (5, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 6: Combo Mì Ý Gà Rán (combo_id = 6)
-- 1 Mì Ý Gà Rán + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (6, 19, 1); -- Mì Ý Gà Rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (6, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 7: Combo Mì Ý và Gà Tender (combo_id = 7)
-- 1 Mì Ý Gà Viên + 2 Miếng Gà Rán Tenders + 1 Khoai Tây Chiên (Vừa) + 1 Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (7, 18, 1); -- Mì Ý Gà Viên
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (7, 11, 1); -- 3 Miếng Gà Rán Tender (thay thế cho 2 miếng)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (7, 26, 1); -- Khoai Tây Chiên (Vừa)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (7, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 8: Combo Mì Ý và Salad Gà (combo_id = 8)
-- 1 Mì Ý Gà Viên + 1 Xà Lách Gà Viên + 1 Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (8, 18, 1); -- Mì Ý Gà Viên
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (8, 22, 1); -- Salad Pop (Xà Lách Gà Viên)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (8, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 9: Combo Cơm Gà Rán (combo_id = 9)
-- 1 Cơm Gà Rán + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (9, 15, 1); -- Cơm Gà Rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (9, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 10: Combo Cơm Gà Quay (combo_id = 10)
-- 1 Cơm Gà Flava + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (10, 16, 1); -- Cơm Phi-lê Gà Quay
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (10, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 11: Combo Cơm Gà Nanban (combo_id = 11)
-- 1 Cơm Gà Nanban + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (11, 20, 1); -- Cơm Gà Viên Nanban
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (11, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 12: Combo Cơm Gà Nanban và Súp Rong Biển (combo_id = 12)
-- 1 Cơm Gà Nanban + 1 Súp Rong Biển + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (12, 20, 1); -- Cơm Gà Viên Nanban
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (12, 35, 1); -- Súp Rong Biển
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (12, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 13: Combo Burger Tôm (combo_id = 13)
-- 1 Burger Tôm + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (13, 14, 1); -- Burger Tôm
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (13, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 14: Combo Burger Gà Zinger và Khoai (combo_id = 14)
-- 1 Burger Zinger + 1 Khoai Tây Chiên (Vừa) + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (14, 13, 1); -- Burger Zinger
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (14, 26, 1); -- Khoai Tây Chiên (Vừa)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (14, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 15: Combo Burger Phi-lê Gà Quay và Khoai (combo_id = 15)
-- 1 Burger Gà Quay + 1 Khoai Tây Chiên (Vừa) + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (15, 15, 1); -- Burger Gà Quay Flava
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (15, 26, 1); -- Khoai Tây Chiên (Vừa)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (15, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 16: Combo Burger Tôm & Gà Rán (combo_id = 16)
-- 1 Burger Tôm + 1 Miếng Gà Rán + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (16, 14, 1); -- Burger Tôm
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (16, 1, 1);  -- 1 Miếng gà rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (16, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 17: Combo Burger Phi-lê Gà Quay và Gà Rán (combo_id = 17)
-- 1 Burger Phi-lê Gà Quay + 1 Miếng Gà Rán + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (17, 15, 1); -- Burger Gà Quay Flava
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (17, 1, 1);  -- 1 Miếng gà rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (17, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 18: Combo Burger Gà Zinger và Gà Rán (combo_id = 18)
-- 1 Burger Zinger + 1 Miếng Gà Rán + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (18, 13, 1); -- Burger Zinger
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (18, 1, 1);  -- 1 Miếng gà rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (18, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 19: Combo Burger và Gà Rán (combo_id = 19)
-- 1 Burger Gà Quay/Zinger + 1 Khoai Tây Chiên (Vừa) + 1 Miếng Gà Rán + 1 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (19, 13, 1); -- Burger Zinger (chọn 1 trong 2)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (19, 26, 1); -- Khoai Tây Chiên (Vừa)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (19, 1, 1);  -- 1 Miếng gà rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (19, 41, 1); -- Pepsi (Tiêu Chuẩn)

-- Combo 20: Combo Nhóm 2 Hoàn Hảo (combo_id = 20)
-- 2 Miếng Gà Rán + 1 Burger Zinger + 2 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (20, 2, 1);  -- 2 Miếng gà rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (20, 13, 1); -- Burger Zinger
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (20, 41, 2); -- Pepsi (Tiêu Chuẩn) x2

-- Combo 21: Combo Nhóm 2 Tròn Vị (combo_id = 21)
-- 3 Miếng Gà Rán + 1 Mì Ý Gà Viên + 2 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (21, 3, 1);  -- 3 Miếng gà rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (21, 18, 1); -- Mì Ý Gà Viên
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (21, 41, 2); -- Pepsi (Tiêu Chuẩn) x2

-- Combo 22: Combo Nhóm 2 No Nê (combo_id = 22)
-- 4 Miếng Gà Rán + 1 Khoai Múi Cau (Vừa) + 2 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (22, 1, 4);  -- 1 Miếng gà rán x4
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (22, 29, 1); -- Khoai Tây Múi Cau (Vừa)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (22, 41, 2); -- Pepsi (Tiêu Chuẩn) x2

-- Combo 23: Combo Nhóm 3 Đủ Đầy (combo_id = 23)
-- 3 Miếng Gà Rán + 1 Mì Ý Gà Viên + 1 Burger Tôm + 1 Khoai Tây Chiên (Vừa) + 3 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (23, 3, 1);  -- 3 Miếng gà rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (23, 18, 1); -- Mì Ý Gà Viên
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (23, 14, 1); -- Burger Tôm
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (23, 26, 1); -- Khoai Tây Chiên (Vừa)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (23, 41, 3); -- Pepsi (Tiêu Chuẩn) x3

-- Combo 24: Combo Nhóm 5 Hội Tụ (combo_id = 24)
-- 6 Miếng Gà Rán + 1 Mì Ý Gà Viên + 1 Khoai Múi Cau (Vừa) + 5 Ly Pepsi (Tiêu chuẩn)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (24, 4, 1);  -- 6 Miếng gà rán
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (24, 18, 1); -- Mì Ý Gà Viên
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (24, 29, 1); -- Khoai Tây Múi Cau (Vừa)
INSERT INTO ComboItem (combo_id, product_id, quantity) VALUES (24, 41, 5); -- Pepsi (Tiêu Chuẩn) x5

-- ===== User =====
INSERT INTO "User" (name, username, password_hash, role) VALUES('Administrator','Admin', '$2a$12$QKJD5hy5YIWy8MVApXvk/uHDn2Cm.SXmGvb5y.PhGH1Ra4FT86HMK', 'admin');
INSERT INTO "User" (name, username, password_hash, role) VALUES('Staff1','Staff1', '$2a$12$EM8WgTDvanSvbwaJcs9uQOwwDvW8j.2kKbiBOMXBeQTE2NJ6fct26', 'staff');

