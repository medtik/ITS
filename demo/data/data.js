const trips = [
    {
        title: "Trải nghiệm Bắc triều tiên", startDate: '25/8/2018', endDate: '25/8/2050', locations: [
            {
                name: 'Bảo tàng chiến tranh Triều Tiên',
                photo: 'http://rs1118.pbsrc.com/albums/k609/LowellBooth/tg_98_tt2.jpg?w=280&h=210&fit=crop'
            },
            {photo: 'http://rsmg.pbsrc.com/albums/v11/polar167/PEI/148_6215.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs94.pbsrc.com/albums/l98/cocco78/Sturgeon%20gorge%20and%20Canyon%20Falls/SturgeonCanyonfalls011.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs730.pbsrc.com/albums/ww310/alboutin07/102_0057.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs1115.pbsrc.com/albums/k552/jshaffer111/Fun%20and%20games/northkorea1.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs580.pbsrc.com/albums/ss241/MrBluJet/MiG15Chino.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs613.pbsrc.com/albums/tt212/sverghese_bucket/SDC10320.jpg?w=280&h=210&fit=crop'}
        ]
    },
    {
        title: "Về quê thằng T", startDate: '23/2/2018', endDate: '26/2/2018', locations: [
            {photo: 'http://rs1085.pbsrc.com/albums/j429/imawakenlove/THOIBINH15-12-2007165-1.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs690.pbsrc.com/albums/vv270/panda_photographer116/P1030617.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs1085.pbsrc.com/albums/j429/imawakenlove/RnguMinhh-CMau081.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs154.pbsrc.com/albums/s277/smeema/Photos%20by%20Smeema%20and%20Waneeta/IMG_3511.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs162.pbsrc.com/albums/t273/alotrip/Visit%20Ben%20Tre%20with%20Vietnam%20Airlines/10178134_1413712558895084_6696279324686284597_n_zps2b0ecad9.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs1085.pbsrc.com/albums/j429/imawakenlove/UM-TB-CM09-01-2011015-1.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs1099.pbsrc.com/albums/g389/nguyen_quang101/TerraceofElephant.jpg?w=280&h=210&fit=crop'}
        ],
    },
    {
        title: "Ẩm thực thái", startDate: '25/8/2018', endDate: '25/8/2050', locations: [
            {photo: 'http://rs598.pbsrc.com/albums/tt65/princessbon3/ThaiOrchid.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs460.pbsrc.com/albums/qq324/KENSETTHEE/f9d47a99.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs242.pbsrc.com/albums/ff231/bour3/Things%20I%20Made%20Then%20Ate%20V/DSC_1384.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs930.pbsrc.com/albums/ad150/mimephotobucket/2010_Journey_4_01_Thailand/DSCF4044_DSC02660_ji.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs282.pbsrc.com/albums/kk247/moldiver_photo/Food%20Blog/DC%20Metro/VA1/Thai%20Old%20Town%20Restaurant/padthai.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs511.pbsrc.com/albums/s356/saknid/Domestic/SaBieng02.jpg?w=280&h=210&fit=crop'},
            {photo: 'http://rs242.pbsrc.com/albums/ff231/bour3/Things%20I%20Made%20Then%20Ate%20V/DSC_1395.jpg?w=280&h=210&fit=crop'}
        ]
    }
];

const members = [
    {name: 'Erika olson', avatar: 'https://randomuser.me/api/portraits/women/84.jpg'},
    {name: 'Rodney lynch', avatar: 'https://randomuser.me/api/portraits/men/21.jpg'},
    {name: 'Herman diaz', avatar: 'https://randomuser.me/api/portraits/men/20.jpg'},
    {name: 'Addison sims', avatar: 'https://randomuser.me/api/portraits/women/47.jpg'},
    {name: 'Arianna olson', avatar: 'https://randomuser.me/api/portraits/women/6.jpg'},
    {name: 'franklin neal', avatar: 'https://randomuser.me/api/portraits/men/46.jpg'},
];

const requests = [
    {
        message: 'Cho một chuyến đi bùng nổ  (╯✧∇✧)╯',
        member: {name: 'Erika olson', avatar: 'https://randomuser.me/api/portraits/women/84.jpg'},
        location: {name: 'Bảo tàng chiến tranh Triều Tiên'},
        trip: {title: "Trải nghiệm Bắc triều tiên"}
    },
    {
        message:'Có người bạn kêu thử chỗ này, ngon bổ rẻ và gần khách sạn mình',
        member: {name: 'Herman diaz', avatar: 'https://randomuser.me/api/portraits/men/20.jpg'},
        location: {name: 'Tealicious Bangkok'},
        trip: {title: "Ẩm thực thái"}
    },
    {
        member: {name: 'Arianna olson', avatar: 'https://randomuser.me/api/portraits/women/6.jpg'},
        location: {name: 'Khu trải nghiệm nông nghiệp'},
        trip: {title: "Trải nghiệm Bắc triều tiên"}
    }
];