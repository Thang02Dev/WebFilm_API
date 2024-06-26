CREATE DATABASE WebFilm
GO
USE [WebFilm]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 06/11/2023 3:25:10 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Position] [int] NULL,
	[Status] [bit] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Slug] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 06/11/2023 3:25:10 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Status] [bit] NOT NULL,
	[Slug] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Episodes]    Script Date: 06/11/2023 3:25:10 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Episodes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MovieId] [int] NULL,
	[Link] [varchar](250) NOT NULL,
	[Episode_Number] [int] NOT NULL,
	[LinkServerId] [int] NULL,
 CONSTRAINT [PK_Episodes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genres]    Script Date: 06/11/2023 3:25:10 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genres](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Status] [bit] NOT NULL,
	[Slug] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LinkServers]    Script Date: 06/11/2023 3:25:10 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LinkServers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_LinkServers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovieGenres]    Script Date: 06/11/2023 3:25:10 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovieGenres](
	[MovieId] [int] NOT NULL,
	[GenreId] [int] NOT NULL,
 CONSTRAINT [PK_MovieGenres] PRIMARY KEY CLUSTERED 
(
	[MovieId] ASC,
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Movies]    Script Date: 06/11/2023 3:25:10 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Trailer] [varchar](200) NULL,
	[Status] [bit] NOT NULL,
	[Resolution] [int] NOT NULL,
	[Subtitle] [bit] NOT NULL,
	[Duration_Minutes] [int] NOT NULL,
	[Image] [nvarchar](250) NOT NULL,
	[Slug] [varchar](250) NOT NULL,
	[CategoryId] [int] NULL,
	[CountryId] [int] NULL,
	[Hot] [bit] NULL,
	[Name_Eng] [varchar](200) NOT NULL,
	[Created_Date] [datetime2](7) NULL,
	[Updated_Date] [datetime2](7) NULL,
	[Year_Release] [varchar](10) NOT NULL,
	[Tags] [nvarchar](max) NULL,
	[Top_View] [bit] NULL,
	[Episode_Number] [int] NULL,
	[Position] [int] NULL,
	[Director] [nvarchar](max) NULL,
	[Performer] [nvarchar](max) NULL,
 CONSTRAINT [PK_Movies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 06/11/2023 3:25:10 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Email] [varchar](200) NOT NULL,
	[Password] [varchar](200) NOT NULL,
	[Status] [bit] NOT NULL,
	[Created_Date] [datetime2](7) NULL,
	[Updated_Date] [datetime2](7) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Views]    Script Date: 06/11/2023 3:25:10 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Views](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MovieId] [int] NOT NULL,
	[ViewerIP] [nvarchar](max) NULL,
	[DateViewed] [datetime2](7) NULL,
 CONSTRAINT [PK_Views] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [Name], [Position], [Status], [Description], [Slug]) VALUES (1, N'Phim bộ', 4, 1, N'abc', N'phim-bo')
INSERT [dbo].[Categories] ([Id], [Name], [Position], [Status], [Description], [Slug]) VALUES (2, N'Phim mới', 3, 1, N'as', N'phim-moi')
INSERT [dbo].[Categories] ([Id], [Name], [Position], [Status], [Description], [Slug]) VALUES (3, N'Quốc gia', 2, 1, N'sadsd', N'quoc-gia')
INSERT [dbo].[Categories] ([Id], [Name], [Position], [Status], [Description], [Slug]) VALUES (14, N'Thể loại', 1, 1, N'sda
', N'the-loai')
INSERT [dbo].[Categories] ([Id], [Name], [Position], [Status], [Description], [Slug]) VALUES (15, N'Phim lẻ', 5, 1, N'danh mục phim lẻ', N'phim-le')
INSERT [dbo].[Categories] ([Id], [Name], [Position], [Status], [Description], [Slug]) VALUES (17, N'Phim chiếu rạp', 7, 1, N'danh mục phim chiếu rạp', N'phim-chieu-rap')
INSERT [dbo].[Categories] ([Id], [Name], [Position], [Status], [Description], [Slug]) VALUES (18, N'TV show', 8, 1, N'danh mục về tv show
', N'tv-show')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([Id], [Name], [Description], [Status], [Slug]) VALUES (3, N'Việt Nam', N'sabcd', 1, N'viet-nam')
INSERT [dbo].[Countries] ([Id], [Name], [Description], [Status], [Slug]) VALUES (4, N'Âu Mỹ', N'châu âu và châu mỹ', 1, N'au-my')
INSERT [dbo].[Countries] ([Id], [Name], [Description], [Status], [Slug]) VALUES (5, N'Hàn Quốc', N'quốc gia phim hàn quốc
', 1, N'han-quoc')
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Episodes] ON 

INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (19, 14, N'https://hd1080.opstream2.com/share/7b6ad2297d3beb569ddf3ee1ce22ffa8', 1, 3)
INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (23, 15, N'https://hdbo.opstream5.com/share/44a64c37e2c5c10ef6f6c45755af9822', 1, 3)
INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (24, 15, N'test', 1, 1)
INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (25, 16, N'https://1080.opstream4.com/share/bd6ccfa862163caf331815f4ec61f1af', 1, 3)
INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (26, 16, N'https://1080.opstream4.com/share/92ebe03166d8ff4b2bbd06077f92fb79', 2, 3)
INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (27, 16, N'https://1080.opstream4.com/share/8f7a73fe6b9682ff2ad7400c0744a043', 3, 3)
INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (28, 16, N'https://hd1080.opstream2.com/share/f18ca68352536e995e0cea95572067d4', 4, 3)
INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (29, 16, N'https://hdbo.opstream5.com/share/303fdade7ec2e86c1be79bf8a8f327f5', 5, 3)
INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (30, 16, N'https://hdbo.opstream5.com/share/67e91755751b124a5e752c5fdcaa3a48', 6, 3)
INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (31, 16, N'https://kd.opstream3.com/share/f5ebd52a0dc78551b072aa2eb23438b8', 7, 3)
INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (32, 17, N'https://1080.opstream4.com/share/425e4714036e70b93d1683fc33b757e6', 1, 3)
INSERT [dbo].[Episodes] ([Id], [MovieId], [Link], [Episode_Number], [LinkServerId]) VALUES (33, 18, N'https://1080.opstream4.com/share/daa178b0fd24b633634b00b9e8781fe2', 1, 3)
SET IDENTITY_INSERT [dbo].[Episodes] OFF
GO
SET IDENTITY_INSERT [dbo].[Genres] ON 

INSERT [dbo].[Genres] ([Id], [Name], [Description], [Status], [Slug]) VALUES (1, N'Phưu lưu-Hành động', N'phim hđ', 1, N'phuu-luu-hanh-dong')
INSERT [dbo].[Genres] ([Id], [Name], [Description], [Status], [Slug]) VALUES (2, N'Tình cảm', N'phim tình cảm abc', 1, N'tinh-cam')
INSERT [dbo].[Genres] ([Id], [Name], [Description], [Status], [Slug]) VALUES (3, N'Khoa học-Viễn tưởng', N'ád', 1, N'khoa-hoc-vien-tuong')
INSERT [dbo].[Genres] ([Id], [Name], [Description], [Status], [Slug]) VALUES (5, N'Hoạt hình', N'thể loại phim hoạt hình', 1, N'hoat-hinh')
INSERT [dbo].[Genres] ([Id], [Name], [Description], [Status], [Slug]) VALUES (6, N'Chính Kịch', N'phim chính kịch', 1, N'chinh-kich')
SET IDENTITY_INSERT [dbo].[Genres] OFF
GO
SET IDENTITY_INSERT [dbo].[LinkServers] ON 

INSERT [dbo].[LinkServers] ([Id], [Name], [Description], [Status]) VALUES (1, N'Vietsub #2', N'server test', 1)
INSERT [dbo].[LinkServers] ([Id], [Name], [Description], [Status]) VALUES (3, N'Vietsub #1', N'nguồn phim từ ophim1.cc
', 1)
SET IDENTITY_INSERT [dbo].[LinkServers] OFF
GO
INSERT [dbo].[MovieGenres] ([MovieId], [GenreId]) VALUES (14, 1)
INSERT [dbo].[MovieGenres] ([MovieId], [GenreId]) VALUES (15, 1)
INSERT [dbo].[MovieGenres] ([MovieId], [GenreId]) VALUES (16, 1)
INSERT [dbo].[MovieGenres] ([MovieId], [GenreId]) VALUES (17, 1)
INSERT [dbo].[MovieGenres] ([MovieId], [GenreId]) VALUES (14, 3)
INSERT [dbo].[MovieGenres] ([MovieId], [GenreId]) VALUES (17, 3)
INSERT [dbo].[MovieGenres] ([MovieId], [GenreId]) VALUES (18, 3)
INSERT [dbo].[MovieGenres] ([MovieId], [GenreId]) VALUES (17, 5)
INSERT [dbo].[MovieGenres] ([MovieId], [GenreId]) VALUES (16, 6)
INSERT [dbo].[MovieGenres] ([MovieId], [GenreId]) VALUES (18, 6)
GO
SET IDENTITY_INSERT [dbo].[Movies] ON 

INSERT [dbo].[Movies] ([Id], [Title], [Description], [Trailer], [Status], [Resolution], [Subtitle], [Duration_Minutes], [Image], [Slug], [CategoryId], [CountryId], [Hot], [Name_Eng], [Created_Date], [Updated_Date], [Year_Release], [Tags], [Top_View], [Episode_Number], [Position], [Director], [Performer]) VALUES (14, N'Bọ Hung Xanh', N'Bọ Hung Xanh kể về cậu sinh viên mới tốt nghiệp Jaime Reyes trở về nhà với tràn trề niềm tin và hy vọng về tương lai, để rồi nhận ra quê nhà của anh đã thay đổi rất nhiều so với trước đây. Khi tìm kiếm mục đích sống trên thế giới này, Jamie đối mặt với bước ngoặt cuộc đời khi anh nhận ra mình sở hữu một di sản cổ đại của công nghệ sinh học ngoài hành tinh: Scarab (Bọ Hung). Khi Scarab chọn Jamie trở thành vật chủ, anh được ban tặng một bộ áo giáp với siêu sức mạnh đáng kinh ngạc không ai có thể lường trước. Số phận của Jamie hoàn toàn thay đổi khi giờ đây, anh đã là siêu anh hùng BLUE BEETLE.', NULL, 1, 3, 1, 128, N'https://i.mpcdn.top/c/B5eM8Qn/bo-hung-xanh.jpg?1695087284', N'bo-hung-xanh', 17, 4, 1, N'Blue Beetle', CAST(N'2023-09-24T21:42:57.5633188' AS DateTime2), CAST(N'2023-09-24T21:44:25.5967778' AS DateTime2), N'2023', N'bo hung xanh, blue beetle', 0, 1, 1, N'Angel Manuel Soto', N'Xolo Maridueña, Bruna Marquezine, George Lopez, Belissa Escobedo, …')
INSERT [dbo].[Movies] ([Id], [Title], [Description], [Trailer], [Status], [Resolution], [Subtitle], [Duration_Minutes], [Image], [Slug], [CategoryId], [CountryId], [Hot], [Name_Eng], [Created_Date], [Updated_Date], [Year_Release], [Tags], [Top_View], [Episode_Number], [Position], [Director], [Performer]) VALUES (15, N'Mồi Quỷ Dữ', N'Mồi Quỷ Dữ xoay quanh sơ Ann (do Jacqueline Byers thủ vai) bị kéo vào một cuộc chiến tại một Nhà Thờ Công Giáo trước thế lực quỷ ám đang ngày một hùng mạnh. Với khả năng chiến đấu với quỷ dữ, sơ Ann được phép thực hiện các buổi trừ tà dẫu cho các luật lệ xưa cũ chỉ cho phép Cha xứ thực hiện công việc này. Cùng với Cha Dante, sơ Ann chạm mặt một con quỷ dữ đang cố chiếm lấy linh hồn của một cô gái trẻ, và cũng có thể là kẻ đã ám lấy người mẹ quá cố của sơ. Sơ Ann dần nhận ra mối nguy đang đe dọa mình khủng khiếp thế nào, và cả lý do con quỷ dữ đó khao khát đoạt mạng cô.', N'', 1, 3, 1, 95, N'https://i.mpcdn.top/poster/moi-quy-du-10857.jpg?1673288682', N'moi-quy-du', 15, 4, 1, N'Prey for the Devil', CAST(N'2023-09-26T15:17:56.1870666' AS DateTime2), CAST(N'2023-10-15T16:45:01.3638348' AS DateTime2), N'2022', N'moi quy du', 0, 1, 3, N'Prey for the Devil', N'Jacqueline Byers, Colin Salmon, Christian Navarro, Lisa Palfrey, Nicholas Ralph, Ben Cross, Virginia Madsen')
INSERT [dbo].[Movies] ([Id], [Title], [Description], [Trailer], [Status], [Resolution], [Subtitle], [Duration_Minutes], [Image], [Slug], [CategoryId], [CountryId], [Hot], [Name_Eng], [Created_Date], [Updated_Date], [Year_Release], [Tags], [Top_View], [Episode_Number], [Position], [Director], [Performer]) VALUES (16, N'GEN V', N'Gen V là bộ phim tách biệt được mong đợi từ loạt phim The Boys của Prime Video, là một phiên bản phản truyền thống về câu chuyện siêu anh hùng trong quá trình đào tạo, mang đến một loạt cười châm biếm sắc bén giống như điểm nhấn khiến cho loạt phim The Boys trở thành một siêu phẩm thành công. Gen V kể về trải nghiệm tại trường đại học Godolkin của thế hệ tiếp theo của các siêu anh hùng ở Mỹ, khi họ đặt sức mạnh, tình dục và đạo đức của mình vào cuộc kiểm tra, cạnh tranh nhau để có được những hợp đồng tốt nhất ở những thành phố tốt nhất.
Với cơn sốt hormone và sức mạnh siêu nhiên đang ngoài tầm kiểm soát do thuốc kích thích sức mạnh Compound V, chúng ta có thể mong đợi những cuộc hỗn loạn thú vị cấp độ R giống như Animal House, những trận đấu sinh tồn kiểu Hunger Games và những cảnh tàn sát máu me cấp độ Carrie thường xuyên. Đoạn trailer đầu tiên của Gen V trông giống như một chuyến tham quan kinh hoàng trên khuôn viên trường đại học. Một con rối được lấy cảm hứng từ Sesame Street bị cắt đầu. Những người lính trang bị sẵn sàng cho chiến tranh nằm chết trong các hành lang. Và những sinh viên năm nhất thường xuyên bị bắn tung máu', N'', 1, 3, 1, 55, N'https://i.mpcdn.top/c/Jak1ZJd/gen-v.jpg?1698986759', N'gen-v', 1, 4, 1, N'The Boys Presents: Varsity (2023)', CAST(N'2023-10-29T17:31:25.0910975' AS DateTime2), CAST(N'2023-11-04T17:18:42.9596486' AS DateTime2), N'2023', N'Gen V, The Boys Presents Varsity', 0, 8, 4, N'Craig Rosenberg, Evan Goldberg, Eric Kripke', N'Jaz Sinclair, Chance Perdomo, Lizze Broadway, Shelley Conn, Maddie Phillips, London Thor, Derek Luh, Asa Germann, Patrick Schwarzenegger, Sean Patrick Thomas')
INSERT [dbo].[Movies] ([Id], [Title], [Description], [Trailer], [Status], [Resolution], [Subtitle], [Duration_Minutes], [Image], [Slug], [CategoryId], [CountryId], [Hot], [Name_Eng], [Created_Date], [Updated_Date], [Year_Release], [Tags], [Top_View], [Episode_Number], [Position], [Director], [Performer]) VALUES (17, N'Người Nhện: Du Hành Vũ Trụ Nhện', N'Người Nhện: Du Hành Vũ Trụ Nhện Miles Morales tái xuất trong phần tiếp theo của bom tấn hoạt hình từng đoạt giải Oscar - Spider-Man: Across the Spider-Verse. Sau khi gặp lại Gwen Stacy, chàng Spider-Man thân thiện đến từ Brooklyn phải du hành qua đa vũ trụ và gặp một nhóm Người Nhện chịu trách nhiệm bảo vệ các thế giới song song. Nhưng khi nhóm siêu anh hùng xung đột về cách xử lý một mối đe dọa mới, Miles buộc phải đọ sức với các Người Nhện khác và phải xác định lại ý nghĩa của việc trở thành một người hùng để có thể cứu những người cậu yêu thương nhất.', N'', 1, 3, 1, 140, N'https://i.mpcdn.top/c/DWWpKoZ/nguoi-nhen-du-hanh-vu-tru.jpg?1688140550', N'nguoi-nhen:-du-hanh-vu-tru-nhen', 17, 4, 1, N'Spider-Man: Across the Spider-Verse', CAST(N'2023-10-29T22:49:09.5149238' AS DateTime2), CAST(N'2023-11-04T17:19:09.9653932' AS DateTime2), N'2023', N'Người Nhện: Du Hành Vũ Trụ Nhện, Xem phim Người Nhện: Du Hành Vũ Trụ Nhện', 0, 1, 2, N'Joaquim Dos Santos, Justin K. Thompson', N'Shameik Moore, Hailee Steinfeld, Jake Johnson, Oscar Isaac, Issa Rae, Daniel Kaluuya, Karan Soni, Jason Schwartzman, Brian Tyree Henry, Luna Lauren Velez')
INSERT [dbo].[Movies] ([Id], [Title], [Description], [Trailer], [Status], [Resolution], [Subtitle], [Duration_Minutes], [Image], [Slug], [CategoryId], [CountryId], [Hot], [Name_Eng], [Created_Date], [Updated_Date], [Year_Release], [Tags], [Top_View], [Episode_Number], [Position], [Director], [Performer]) VALUES (18, N'Địa Đàng Sụp Đổ', N'Thế giới đã trở thành đống đổ nát sau một trận động đất lớn. Trong khi không ai biết chắc chắn tàn tích trải dài bao xa, hay nguyên nhân gây ra trận động đất là gì, thì ở trung tâm Seoul chỉ còn lại một tòa nhà chung cư. Nó được gọi là Căn hộ Hwang Gung.', NULL, 1, 3, 1, 130, N'https://pm-focus-opensocial.googleusercontent.com/gadgets/proxy?container=focus&refresh=604800&url=https%3A%2F%2Fphimmoi.im%2Fstorage%2Fimages%2Fdia-dang-sup-do%2Fb1-2023-08-25t210055037-1692972358.jpg', N'dia-dang-sup-do', 15, 5, 1, N'Concrete Utopia', CAST(N'2023-10-29T22:57:22.9958864' AS DateTime2), CAST(N'2023-11-04T17:20:25.0008364' AS DateTime2), N'2023', NULL, 0, 1, 4, N'Um Tae-hwa', N'Lee Byung-hun, Park Seo-jun, Park Bo-young, Kim Sun-young, Park Ji-hu, Kim Do-yoon, Lee Seo-hwan, Kang Ae-sim, Lee Hyo-je, Kim Shi-un')
SET IDENTITY_INSERT [dbo].[Movies] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Name], [Email], [Password], [Status], [Created_Date], [Updated_Date]) VALUES (2, N'Thắng', N'admin@admin.com', N'$2a$11$f9G0xdyfccQ2A80zIGL4e.OlBEd9UMXkd52qHWT5x9TktzrmzgNcO', 1, CAST(N'2023-10-27T01:06:04.2448975' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[Views] ON 

INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (7, 15, N'172.16.0.2', CAST(N'2023-10-21T00:09:04.5239350' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (8, 15, N'172.16.0.2', CAST(N'2023-10-21T00:09:07.8695403' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (9, 14, N'172.16.0.2', CAST(N'2023-10-21T00:09:11.8380030' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (10, 15, N'172.16.0.2', CAST(N'2023-10-21T00:53:19.5184094' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (11, 14, N'192.168.0.3', CAST(N'2023-10-25T15:22:54.2282172' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (12, 15, N'192.168.0.107', CAST(N'2023-10-25T22:05:55.2445430' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (13, 14, N'192.168.0.3', CAST(N'2023-10-29T01:00:40.2353646' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (14, 16, N'172.16.0.2', CAST(N'2023-10-29T17:41:34.9011237' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (15, 16, N'172.16.0.2', CAST(N'2023-10-29T18:33:57.0171585' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (16, 16, N'172.16.0.2', CAST(N'2023-10-29T22:29:59.2861350' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (17, 16, N'172.16.0.2', CAST(N'2023-10-29T22:32:17.2341048' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (18, 16, N'172.16.0.2', CAST(N'2023-10-29T22:32:24.9288873' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (19, 16, N'172.16.0.2', CAST(N'2023-10-29T22:32:30.6860218' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (20, 16, N'172.16.0.2', CAST(N'2023-10-29T22:32:35.1729860' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (21, 16, N'172.16.0.2', CAST(N'2023-10-29T22:39:49.9881751' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (22, 17, N'172.16.0.2', CAST(N'2023-10-29T08:49:49.0155402' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (23, 18, N'172.16.0.2', CAST(N'2023-10-29T22:58:39.2463016' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (24, 16, N'172.16.0.2', CAST(N'2023-10-29T23:10:09.6394132' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (25, 14, N'172.23.96.1', CAST(N'2023-11-01T22:57:15.4753142' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (26, 16, N'127.0.0.1', CAST(N'2023-11-01T22:59:08.9503603' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (27, 16, N'172.23.96.1', CAST(N'2023-11-01T23:05:52.2190527' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (28, 14, N'192.168.213.19', CAST(N'2023-11-03T16:12:06.3830389' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (29, 16, N'192.168.77.19', CAST(N'2023-11-03T16:17:09.2183235' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (30, 16, N'192.168.0.107', CAST(N'2023-11-04T17:22:27.3449862' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (31, 18, N'192.168.0.107', CAST(N'2023-11-04T17:30:45.1587882' AS DateTime2))
INSERT [dbo].[Views] ([Id], [MovieId], [ViewerIP], [DateViewed]) VALUES (32, 18, N'192.168.0.107', CAST(N'2023-11-04T20:13:02.2497164' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Views] OFF
GO
ALTER TABLE [dbo].[Episodes]  WITH CHECK ADD  CONSTRAINT [FK_Episodes_LinkServers_LinkServerId] FOREIGN KEY([LinkServerId])
REFERENCES [dbo].[LinkServers] ([Id])
GO
ALTER TABLE [dbo].[Episodes] CHECK CONSTRAINT [FK_Episodes_LinkServers_LinkServerId]
GO
ALTER TABLE [dbo].[Episodes]  WITH CHECK ADD  CONSTRAINT [FK_Episodes_Movies_MovieId] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([Id])
GO
ALTER TABLE [dbo].[Episodes] CHECK CONSTRAINT [FK_Episodes_Movies_MovieId]
GO
ALTER TABLE [dbo].[MovieGenres]  WITH CHECK ADD  CONSTRAINT [FK_MovieGenres_Genres_GenreId] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genres] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MovieGenres] CHECK CONSTRAINT [FK_MovieGenres_Genres_GenreId]
GO
ALTER TABLE [dbo].[MovieGenres]  WITH CHECK ADD  CONSTRAINT [FK_MovieGenres_Movies_MovieId] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MovieGenres] CHECK CONSTRAINT [FK_MovieGenres_Movies_MovieId]
GO
ALTER TABLE [dbo].[Movies]  WITH CHECK ADD  CONSTRAINT [FK_Movies_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[Movies] CHECK CONSTRAINT [FK_Movies_Categories_CategoryId]
GO
ALTER TABLE [dbo].[Movies]  WITH CHECK ADD  CONSTRAINT [FK_Movies_Countries_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Movies] CHECK CONSTRAINT [FK_Movies_Countries_CountryId]
GO
ALTER TABLE [dbo].[Views]  WITH CHECK ADD  CONSTRAINT [FK_Views_Movies_MovieId] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Views] CHECK CONSTRAINT [FK_Views_Movies_MovieId]
GO
